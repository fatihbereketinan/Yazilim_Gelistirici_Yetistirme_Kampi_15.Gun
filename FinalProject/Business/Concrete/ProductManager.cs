using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal; //Bir entity manager kendisi hariç başka bir dalı enjekte edemez. ICategoryDal gibi.
        ICategoryService _categoryService; //Servis deme sebebimiz bundan geliyor. Servise ait metotları kullanacağız.
        //Edevlet doğrulamasınıda servis şeklinde enjekte edeceğiz.
        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService= categoryService;
        }
        [SecuredOperation("product.add,admin")] //korunan operasyon demek. Bu metodu çağırabilecek yetkililer parantez içinde.
        [ValidationAspect(typeof(ProductValidator))] //Add metodumuza doğrulama kodlarımızı aspect ekledik. 
        [CacheRemoveAspect("IProductService.Get")]
        [TransactionScopeAspect]
        public IResult Add(Product product)
        {
            //business codes
            IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName),
                CheckIfProductCountOfCategoryCorrect(product.CategoryId), CheckIfCategoryLimitExceded());
            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);
        }

        [CacheAspect] //Cache lemek istediğimiz datayı key,value çifti ile tutarız.
        [PerformanceAspect(5)] //Bu method'un çalışması 5sn'i geçerse uyarı verir
        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 22)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);

        }
        [CacheAspect]
        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }
        [CacheAspect]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        [CacheAspect]
        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        [TransactionScopeAspect]
        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult();
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IProductService.Get")]
        [TransactionScopeAspect]
        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult(Messages.ProductDeleted);
        }

        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            //Bir kategoride en fazla 10 ürün olabilir.
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }
        private IResult CheckIfProductNameExists(string productName)
        {
            //Aynı isimde ürün eklenemeyecek.
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();  //Any-->system linq
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }
        private IResult CheckIfCategoryLimitExceded()
        {
            //Eğer mevcut kategori sayısı 15'i geçtiyse sisteme yeni ürün eklenemesin.
            var result = _categoryService.GetAll();
            if (result.Data.Count>15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }
    }
}

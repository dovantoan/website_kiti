using AutoMapper;
using EntityLibrary;
using EntityLibrary.ModuleImplement;
using titi.Areas.Admin.Models;
using titi.Areas.Admin.SearchCriteria;
using titi.Models.Account;
using titi.Models.MasterModels;
using Util.SearchEntity;

namespace titi.Infrastructure
{
    public class MappingUtil
    {
        public static void MappingInitialize()
        {
            Mapper.Initialize(cfg =>
                cfg.AddProfiles(new[] {
                    "titi",
                    "EntityLibrary"
                }));
        }
        public class AutomapperWebProfile : Profile
        {
            public AutomapperWebProfile()
            {
                CreateMap<Category, Models.CategoryModel>().ReverseMap();
                CreateMap<User, UserModel>().ReverseMap();
                CreateMap<UserModel, User>().ReverseMap();
                CreateMap<User, UserLoginProfile>().ReverseMap();
                CreateMap<RoleImplement, RoleViewModel>().ReverseMap();
                CreateMap<DefineUI, DefineUIModel>().ReverseMap();
                CreateMap<DefineUIModel, DefineUI>().ReverseMap();
                CreateMap<Tokens, TokenModel>().ReverseMap();
                CreateMap<ProductSearchCriteria, ProductSearchEntity>().ReverseMap();
                CreateMap<Product, ProductModel>().ReverseMap();
                CreateMap<Product, Models.ProductModel>().ReverseMap();
                CreateMap<Category, CategoryModel>().ReverseMap();
                CreateMap<CategoryModel, Category>().ReverseMap();
                CreateMap<Producer, ProducerModel>().ReverseMap();
                CreateMap<ProducerModel, Producer>().ReverseMap();
                CreateMap<Role, RoleModel>().ReverseMap();
                CreateMap<RoleModel, Role>().ReverseMap();
                CreateMap<Group,GroupModule>().ReverseMap();

                CreateMap<DistributorModels, Distributor>().ReverseMap();
                CreateMap<Distributor, DistributorModels>().ReverseMap();

                CreateMap<Province, ProvinceModels>().ReverseMap();
                CreateMap<District, DistrictModels>().ReverseMap();
                CreateMap<Ward, WardModels>().ReverseMap();
                CreateMap<ShippingModels, Shipping>().ReverseMap();
                CreateMap<Shipping, ShippingModels>().ReverseMap();
                CreateMap<ProductDetail, ProductDetailModel>().ReverseMap();
                CreateMap<ProductDetailModel, ProductDetail>().ReverseMap();
                CreateMap<ProductDetail, titi.Models.ProductDetailModel>().ReverseMap();
            }
        }
    }
}
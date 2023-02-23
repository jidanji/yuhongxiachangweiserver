using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;
using DTOModel;
using DataAccess;


namespace yuhongxiadataServer.Models
{
    public class AutoMapperConfig
    {
        public static void Config()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<IngredientsInWhichDrugs_View, UI_IngredientTable>();
                cfg.CreateMap<UI_IngredientTable, IngredientsInWhichDrugs_View>();


                cfg.CreateMap<IngredientTable, UI_IngredientTable>();
                cfg.CreateMap<UI_IngredientTable, IngredientTable>();


                cfg.CreateMap<UI_DrugTable, DrugTable>();
                cfg.CreateMap<DrugTable, UI_DrugTable>();

                cfg.CreateMap<UI_DrugIngredientRelationship, DrugIngredientRelationship>();
                cfg.CreateMap<DrugIngredientRelationship, UI_DrugIngredientRelationship>();
                

            });
        }
    }
}
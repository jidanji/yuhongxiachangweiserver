﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class yuhongxiadataEntities : DbContext
    {
        public yuhongxiadataEntities()
            : base("name=yuhongxiadataEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DrugIngredientRelationship> DrugIngredientRelationship { get; set; }
        public virtual DbSet<DrugTable> DrugTable { get; set; }
        public virtual DbSet<IngredientTable> IngredientTable { get; set; }
        public virtual DbSet<IngredientsInWhichDrugs_View> IngredientsInWhichDrugs_View { get; set; }
    }
}

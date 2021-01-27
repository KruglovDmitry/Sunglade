namespace TestFlight.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Baskets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        BasketDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BasketElements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        ProductPhoto = c.String(maxLength: 255),
                        ProductName = c.String(maxLength: 255),
                        Price = c.Single(nullable: false),
                        Quant = c.Short(nullable: false),
                        Basket_Id = c.Int(),
                        Order_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Baskets", t => t.Basket_Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .Index(t => t.Basket_Id)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 255),
                        Name = c.String(maxLength: 255),
                        SurName = c.String(maxLength: 255),
                        Email = c.String(maxLength: 255),
                        Photo = c.String(maxLength: 255),
                        City = c.String(),
                        Street = c.String(maxLength: 255),
                        HomeNr = c.String(maxLength: 255),
                        FlatNr = c.String(maxLength: 255),
                        Phone = c.String(maxLength: 255),
                        RegDate = c.DateTime(nullable: false),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Photo = c.String(maxLength: 255),
                        Header = c.String(nullable: false, maxLength: 255),
                        Body = c.String(nullable: false, maxLength: 255),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        ActualName = c.String(maxLength: 255),
                        ActualSurName = c.String(maxLength: 255),
                        ActualPhone = c.String(maxLength: 255),
                        ActualEmail = c.String(maxLength: 255),
                        ActualStreet = c.String(maxLength: 255),
                        ActualHomeNr = c.String(maxLength: 255),
                        ActualFlatNr = c.String(maxLength: 255),
                        BasketId = c.Int(nullable: false),
                        Total = c.Single(nullable: false),
                        OrderDetail = c.String(maxLength: 255),
                        OrderTime = c.DateTime(nullable: false),
                        DeliveryASAP = c.Boolean(nullable: false),
                        DelivaryToTime = c.DateTime(nullable: false),
                        PayByCash = c.Boolean(nullable: false),
                        PayByCard = c.Boolean(nullable: false),
                        Done = c.Boolean(nullable: false),
                        IsAutontificated = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProdTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RusName = c.String(nullable: false, maxLength: 255),
                        EngName = c.String(nullable: false, maxLength: 255),
                        Icon = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProdTypeId = c.Short(nullable: false),
                        SubTypeId = c.Short(nullable: false),
                        ThSubTypeId = c.Short(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        ProdTypeName = c.String(nullable: false, maxLength: 255),
                        Price = c.Int(nullable: false),
                        PriceFor = c.String(nullable: false, maxLength: 255),
                        OldPrice = c.Int(),
                        Photo = c.String(maxLength: 255),
                        Description = c.String(nullable: false, maxLength: 255),
                        Finder = c.String(nullable: false, maxLength: 255),
                        DiscountProd = c.Boolean(nullable: false),
                        Popularity = c.Byte(nullable: false),
                        CrDate = c.DateTime(nullable: false),
                        Avail = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        CustomerName = c.String(maxLength: 255),
                        CustomerSurName = c.String(maxLength: 255),
                        CustomerPhoto = c.String(maxLength: 255),
                        Date = c.DateTime(nullable: false),
                        Body = c.String(maxLength: 255),
                        Score = c.Byte(nullable: false),
                        Valid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SubTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProdTypeId = c.Int(nullable: false),
                        SubTypeName = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ThSubTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubTypeId = c.Int(nullable: false),
                        SubTypeName = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Vacancies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        Shedule = c.String(nullable: false, maxLength: 255),
                        Requirements = c.String(nullable: false, maxLength: 255),
                        Salary = c.Short(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.BasketElements", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.BasketElements", "Basket_Id", "dbo.Baskets");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.BasketElements", new[] { "Order_Id" });
            DropIndex("dbo.BasketElements", new[] { "Basket_Id" });
            DropTable("dbo.Vacancies");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ThSubTypes");
            DropTable("dbo.SubTypes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Reviews");
            DropTable("dbo.Products");
            DropTable("dbo.ProdTypes");
            DropTable("dbo.Orders");
            DropTable("dbo.News");
            DropTable("dbo.Customers");
            DropTable("dbo.BasketElements");
            DropTable("dbo.Baskets");
        }
    }
}

namespace TestFlight.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Final : DbMigration
    {
        public override void Up()
        {
            Sql("SET IDENTITY_INSERT ProdTypes ON");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (0,N'Выпечка','bakery','bread')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (1,N'Фрукты и Овощи','fruits','apple')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (2,N'Молочная продукция и Яица','dairy','cheese')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (3,N'Мясо и Птица','meat','ham-leg')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (4,N'Рыба и Морепродукты','fish','fish')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (5,N'Соусы и специи','sauces','sauce')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (6,N'Консервы и Масло','canned','canned-food')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (7,N'Алкоголь','alcoholic','wine')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (8,N'Безалкогольные напитки и Соки','drinks','juice')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (9,N'Крупы','packets','corn')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (10,N'Замороженное','frozen','frozen')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (11,N'Средства гигиены','hygiene','toothbrush')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (12,N'Кухонные принадлежности','kitchenware','pot')");
            Sql("SET IDENTITY_INSERT ProdTypes OFF");

            Sql("SET IDENTITY_INSERT SubTypes ON");
            Sql("INSERT INTO SubTypes (Id, ProdTypeId, SubTypeName) VALUES (0,0,N'Хлеб')");
            Sql("INSERT INTO SubTypes (Id, ProdTypeId, SubTypeName) VALUES (1,0,N'Булочки')");
            Sql("INSERT INTO SubTypes (Id, ProdTypeId, SubTypeName) VALUES (2,0,N'Питта и Лепешки')");
            Sql("SET IDENTITY_INSERT SubTypes OFF");

            Sql("SET IDENTITY_INSERT ThSubTypes ON");
            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (0,0,N'Багет')");
            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (1,0,N'Буханка')");
            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (2,0,N'Чиабатта')");
            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (3,0,N'Пшеничный хлеб')");
            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (4,0,N'Кукурузный хлеб')");
            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (5,0,N'Ржаной хлеб')");

            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (6,1,N'Слоеное тесто')");
            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (7,1,N'Фаршированные булочки')");
            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (8,1,N'Сладкие булочки')");
            Sql("SET IDENTITY_INSERT ThSubTypes OFF");
        }
        
        public override void Down()
        {
        }
    }
}

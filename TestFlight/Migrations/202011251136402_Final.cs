namespace TestFlight.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Final : DbMigration
    {
        public override void Up()
        {
            Sql("SET IDENTITY_INSERT ProdTypes ON");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (0,N'�������','bakery','bread')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (1,N'������ � �����','fruits','apple')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (2,N'�������� ��������� � ����','dairy','cheese')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (3,N'���� � �����','meat','ham-leg')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (4,N'���� � ������������','fish','fish')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (5,N'����� � ������','sauces','sauce')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (6,N'�������� � �����','canned','canned-food')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (7,N'��������','alcoholic','wine')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (8,N'�������������� ������� � ����','drinks','juice')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (9,N'�����','packets','corn')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (10,N'������������','frozen','frozen')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (11,N'�������� �������','hygiene','toothbrush')");
            Sql("INSERT INTO ProdTypes (Id, RusName, EngName, Icon) VALUES (12,N'�������� ��������������','kitchenware','pot')");
            Sql("SET IDENTITY_INSERT ProdTypes OFF");

            Sql("SET IDENTITY_INSERT SubTypes ON");
            Sql("INSERT INTO SubTypes (Id, ProdTypeId, SubTypeName) VALUES (0,0,N'����')");
            Sql("INSERT INTO SubTypes (Id, ProdTypeId, SubTypeName) VALUES (1,0,N'�������')");
            Sql("INSERT INTO SubTypes (Id, ProdTypeId, SubTypeName) VALUES (2,0,N'����� � �������')");
            Sql("SET IDENTITY_INSERT SubTypes OFF");

            Sql("SET IDENTITY_INSERT ThSubTypes ON");
            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (0,0,N'�����')");
            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (1,0,N'�������')");
            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (2,0,N'��������')");
            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (3,0,N'��������� ����')");
            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (4,0,N'���������� ����')");
            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (5,0,N'������ ����')");

            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (6,1,N'������� �����')");
            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (7,1,N'������������� �������')");
            Sql("INSERT INTO ThSubTypes (Id, SubTypeId, SubTypeName) VALUES (8,1,N'������� �������')");
            Sql("SET IDENTITY_INSERT ThSubTypes OFF");
        }
        
        public override void Down()
        {
        }
    }
}

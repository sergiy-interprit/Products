using Microsoft.EntityFrameworkCore.Migrations;

namespace Products.Data.Migrations
{
	public partial class SeedAccountsAndProductsTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder
				.Sql("INSERT INTO Accounts (Name, Description) VALUES ('Account1', 'Description of Account1.')");
			migrationBuilder
				.Sql("INSERT INTO Accounts (Name, Description) VALUES ('Account2', 'Description of Account2.')");

			migrationBuilder
				.Sql("INSERT INTO Products (Name, Description, Quantity, UnitPrice, AccountId) " +
					"VALUES ('Product1', 'Description of Product1.', 5, 2.1, (SELECT Id FROM Accounts WHERE Name = 'Account1'))");
			migrationBuilder
				.Sql("INSERT INTO Products (Name, Description,  Quantity, UnitPrice, AccountId) " +
					"VALUES ('Product2', 'Description of Product2.', 6, 15.7, (SELECT Id FROM Accounts WHERE Name = 'Account1'))");

			migrationBuilder
				.Sql("INSERT INTO Products (Name, Description,  Quantity, UnitPrice, AccountId) " +
					"VALUES ('Product3', 'Description of Product3.', 10, 18.4, (SELECT Id FROM Accounts WHERE Name = 'Account2'))");
			migrationBuilder
				.Sql("INSERT INTO Products (Name, Description,  Quantity, UnitPrice, AccountId) " +
					"VALUES ('Product4', 'Description of Product4.', 11, 19.3, (SELECT Id FROM Accounts WHERE Name = 'Account2'))");
			migrationBuilder
				.Sql("INSERT INTO Products (Name, Description,  Quantity, UnitPrice, AccountId) " +
					"VALUES ('Product5', 'Description of Product5.', 12, 20.9, (SELECT Id FROM Accounts WHERE Name = 'Account2'))");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder
				.Sql("DELETE FROM Products");

			migrationBuilder
				.Sql("DELETE FROM Accounts");
		}
	}
}

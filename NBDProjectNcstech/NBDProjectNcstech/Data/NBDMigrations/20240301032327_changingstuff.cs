using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NBDProjectNcstech.Data.NBDMigrations
{
    /// <inheritdoc />
    public partial class changingstuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "MaterialRequirments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MaterialRequirments");

            migrationBuilder.DropColumn(
                name: "SizeH",
                table: "MaterialRequirments");

            migrationBuilder.DropColumn(
                name: "SizeL",
                table: "MaterialRequirments");

            migrationBuilder.DropColumn(
                name: "SizeW",
                table: "MaterialRequirments");

            migrationBuilder.RenameColumn(
                name: "SalePrice",
                table: "MaterialRequirments",
                newName: "ExtendedPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "Inventory",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Inventory",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Inventory",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "ExtendedPrice",
                table: "MaterialRequirments",
                newName: "SalePrice");

            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "MaterialRequirments",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MaterialRequirments",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SizeH",
                table: "MaterialRequirments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SizeL",
                table: "MaterialRequirments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SizeW",
                table: "MaterialRequirments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFlowAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Tabelas sem FK (lookup / domínio) ──────────────────────────────

            migrationBuilder.CreateTable(
                name: "SPECIES",
                columns: table => new
                {
                    ID          = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                       .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME        = table.Column<string>(type: "NVARCHAR2(50)",  maxLength: 50,  nullable: false),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_SPECIES", x => x.ID));

            migrationBuilder.CreateTable(
                name: "EVENT_TYPE",
                columns: table => new
                {
                    ID           = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME         = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    POINTS_REWARD = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CATEGORY     = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_EVENT_TYPE", x => x.ID));

            migrationBuilder.CreateTable(
                name: "REWARD_ACTION",
                columns: table => new
                {
                    ID           = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME         = table.Column<string>(type: "NVARCHAR2(50)",  maxLength: 50,  nullable: false),
                    POINTS_VALUE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DESCRIPTION  = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_REWARD_ACTION", x => x.ID));

            migrationBuilder.CreateTable(
                name: "RISK_LEVEL",
                columns: table => new
                {
                    ID          = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                       .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME        = table.Column<string>(type: "NVARCHAR2(50)",  maxLength: 50,  nullable: false),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    MIN_SCORE   = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MAX_SCORE   = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_RISK_LEVEL", x => x.ID));

            // ── Entidades principais ───────────────────────────────────────────

            migrationBuilder.CreateTable(
                name: "TUTOR",
                columns: table => new
                {
                    ID            = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                         .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME          = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    EMAIL         = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    PHONE         = table.Column<string>(type: "NVARCHAR2(20)",  maxLength: 20,  nullable: true),
                    PASSWORD_HASH = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    CREATED_AT    = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_TUTOR", x => x.ID));

            migrationBuilder.CreateTable(
                name: "CLINIC",
                columns: table => new
                {
                    ID         = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                      .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME       = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ADDRESS    = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    PHONE      = table.Column<string>(type: "NVARCHAR2(20)",  maxLength: 20,  nullable: true),
                    CNPJ       = table.Column<string>(type: "NVARCHAR2(18)",  maxLength: 18,  nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_CLINIC", x => x.ID));

            // ── Filhos de TUTOR ────────────────────────────────────────────────

            migrationBuilder.CreateTable(
                name: "ADDRESS",
                columns: table => new
                {
                    ID       = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                    .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TUTOR_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    STREET   = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    CITY     = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    STATE    = table.Column<string>(type: "NVARCHAR2(50)",  maxLength: 50,  nullable: false),
                    ZIP_CODE = table.Column<string>(type: "NVARCHAR2(10)",  maxLength: 10,  nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADDRESS", x => x.ID);
                    table.ForeignKey("FK_ADDRESS_TUTOR", x => x.TUTOR_ID, "TUTOR", "ID");
                });

            migrationBuilder.CreateTable(
                name: "PET",
                columns: table => new
                {
                    ID         = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                      .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TUTOR_ID   = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    SPECIES_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    NAME       = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    BREED      = table.Column<string>(type: "NVARCHAR2(50)",  maxLength: 50,  nullable: true),
                    BIRTH_DATE = table.Column<DateTime>(type: "DATE", nullable: true),
                    WEIGHT     = table.Column<decimal>(type: "NUMBER(5,2)", precision: 5, scale: 2, nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PET", x => x.ID);
                    table.ForeignKey("FK_PET_TUTOR",   x => x.TUTOR_ID,   "TUTOR",   "ID");
                    table.ForeignKey("FK_PET_SPECIES", x => x.SPECIES_ID, "SPECIES", "ID");
                });

            migrationBuilder.CreateTable(
                name: "REWARD_POINT",
                columns: table => new
                {
                    ID               = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                            .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TUTOR_ID         = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    REWARD_ACTION_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    POINTS           = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    REFERENCE_TYPE   = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    REFERENCE_ID     = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_AT       = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REWARD_POINT", x => x.ID);
                    table.ForeignKey("FK_REWARD_POINT_TUTOR",  x => x.TUTOR_ID,         "TUTOR",         "ID");
                    table.ForeignKey("FK_REWARD_POINT_ACTION", x => x.REWARD_ACTION_ID, "REWARD_ACTION", "ID");
                });

            // ── Filhos de CLINIC ───────────────────────────────────────────────

            migrationBuilder.CreateTable(
                name: "PLAN",
                columns: table => new
                {
                    ID             = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                         .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CLINIC_ID      = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    NAME           = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DESCRIPTION    = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    PRICE          = table.Column<decimal>(type: "NUMBER(10,2)", precision: 10, scale: 2, nullable: false),
                    DURATION_DAYS  = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    POINTS_PER_EVENT = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLAN", x => x.ID);
                    table.ForeignKey("FK_PLAN_CLINIC", x => x.CLINIC_ID, "CLINIC", "ID");
                });

            migrationBuilder.CreateTable(
                name: "PARTNER_DISCOUNT",
                columns: table => new
                {
                    ID               = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                            .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CLINIC_ID        = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PARTNER_NAME     = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CATEGORY         = table.Column<string>(type: "NVARCHAR2(50)",  maxLength: 50,  nullable: false),
                    DISCOUNT_PERCENT = table.Column<decimal>(type: "NUMBER(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PARTNER_DISCOUNT", x => x.ID);
                    table.ForeignKey("FK_PARTNER_DISCOUNT_CLINIC", x => x.CLINIC_ID, "CLINIC", "ID");
                });

            // ── Filhos de PET ──────────────────────────────────────────────────

            migrationBuilder.CreateTable(
                name: "HEALTH_EVENT",
                columns: table => new
                {
                    ID            = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                         .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PET_ID        = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    EVENT_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CLINIC_ID     = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DESCRIPTION   = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    EVENT_DATE    = table.Column<DateTime>(type: "DATE", nullable: false),
                    STATUS        = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    CREATED_AT    = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HEALTH_EVENT", x => x.ID);
                    table.ForeignKey("FK_HEALTH_EVENT_PET",    x => x.PET_ID,        "PET",        "ID");
                    table.ForeignKey("FK_HEALTH_EVENT_TYPE",   x => x.EVENT_TYPE_ID, "EVENT_TYPE", "ID");
                    table.ForeignKey("FK_HEALTH_EVENT_CLINIC", x => x.CLINIC_ID,     "CLINIC",     "ID");
                });

            migrationBuilder.CreateTable(
                name: "SUBSCRIPTION",
                columns: table => new
                {
                    ID         = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                      .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PET_ID     = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PLAN_ID    = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    START_DATE = table.Column<DateTime>(type: "DATE", nullable: false),
                    END_DATE   = table.Column<DateTime>(type: "DATE", nullable: true),
                    STATUS     = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUBSCRIPTION", x => x.ID);
                    table.ForeignKey("FK_SUBSCRIPTION_PET",  x => x.PET_ID,  "PET",  "ID");
                    table.ForeignKey("FK_SUBSCRIPTION_PLAN", x => x.PLAN_ID, "PLAN", "ID");
                });

            migrationBuilder.CreateTable(
                name: "RISK_SCORE",
                columns: table => new
                {
                    ID            = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                         .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PET_ID        = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    SCORE         = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    RISK_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CALCULATED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_SCORE", x => x.ID);
                    table.ForeignKey("FK_RISK_SCORE_PET",   x => x.PET_ID,        "PET",        "ID");
                    table.ForeignKey("FK_RISK_SCORE_LEVEL", x => x.RISK_LEVEL_ID, "RISK_LEVEL", "ID");
                });

            // ── Cupons ─────────────────────────────────────────────────────────

            migrationBuilder.CreateTable(
                name: "COUPON_TEMPLATE",
                columns: table => new
                {
                    ID                  = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                               .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PARTNER_DISCOUNT_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    TITLE               = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DISCOUNT_VALUE      = table.Column<decimal>(type: "NUMBER(10,2)", precision: 10, scale: 2, nullable: false),
                    DISCOUNT_TYPE       = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    POINTS_REQUIRED     = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COUPON_TEMPLATE", x => x.ID);
                    table.ForeignKey("FK_COUPON_TEMPLATE_PARTNER", x => x.PARTNER_DISCOUNT_ID, "PARTNER_DISCOUNT", "ID");
                });

            migrationBuilder.CreateTable(
                name: "COUPON",
                columns: table => new
                {
                    ID              = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                          .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TEMPLATE_ID     = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CODE            = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    STATUS          = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    EXPIRATION_DATE = table.Column<DateTime>(type: "DATE", nullable: true),
                    CREATED_AT      = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COUPON", x => x.ID);
                    table.ForeignKey("FK_COUPON_TEMPLATE", x => x.TEMPLATE_ID, "COUPON_TEMPLATE", "ID");
                });

            migrationBuilder.CreateTable(
                name: "REDEEM",
                columns: table => new
                {
                    ID          = table.Column<long>(type: "NUMBER(19)", nullable: false)
                                       .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TUTOR_ID    = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    COUPON_ID   = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    POINTS_USED = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CREATED_AT  = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REDEEM", x => x.ID);
                    table.ForeignKey("FK_REDEEM_TUTOR",  x => x.TUTOR_ID,  "TUTOR",  "ID");
                    table.ForeignKey("FK_REDEEM_COUPON", x => x.COUPON_ID, "COUPON", "ID");
                });

            // ── Índices ────────────────────────────────────────────────────────
            migrationBuilder.CreateIndex("IX_ADDRESS_TUTOR_ID",       "ADDRESS",          "TUTOR_ID");
            migrationBuilder.CreateIndex("IX_PET_TUTOR_ID",           "PET",              "TUTOR_ID");
            migrationBuilder.CreateIndex("IX_PET_SPECIES_ID",         "PET",              "SPECIES_ID");
            migrationBuilder.CreateIndex("IX_PLAN_CLINIC_ID",         "PLAN",             "CLINIC_ID");
            migrationBuilder.CreateIndex("IX_PARTNER_CLINIC_ID",      "PARTNER_DISCOUNT", "CLINIC_ID");
            migrationBuilder.CreateIndex("IX_HEALTH_EVENT_PET_ID",    "HEALTH_EVENT",     "PET_ID");
            migrationBuilder.CreateIndex("IX_HEALTH_EVENT_TYPE_ID",   "HEALTH_EVENT",     "EVENT_TYPE_ID");
            migrationBuilder.CreateIndex("IX_HEALTH_EVENT_CLINIC_ID", "HEALTH_EVENT",     "CLINIC_ID");
            migrationBuilder.CreateIndex("IX_SUBSCRIPTION_PET_ID",    "SUBSCRIPTION",     "PET_ID");
            migrationBuilder.CreateIndex("IX_SUBSCRIPTION_PLAN_ID",   "SUBSCRIPTION",     "PLAN_ID");
            migrationBuilder.CreateIndex("IX_RISK_SCORE_PET_ID",      "RISK_SCORE",       "PET_ID");
            migrationBuilder.CreateIndex("IX_RISK_SCORE_LEVEL_ID",    "RISK_SCORE",       "RISK_LEVEL_ID");
            migrationBuilder.CreateIndex("IX_REWARD_POINT_TUTOR_ID",  "REWARD_POINT",     "TUTOR_ID");
            migrationBuilder.CreateIndex("IX_REWARD_POINT_ACTION_ID", "REWARD_POINT",     "REWARD_ACTION_ID");
            migrationBuilder.CreateIndex("IX_COUPON_TMPL_PD_ID",      "COUPON_TEMPLATE",  "PARTNER_DISCOUNT_ID");
            migrationBuilder.CreateIndex("IX_COUPON_TEMPLATE_ID",     "COUPON",           "TEMPLATE_ID");
            migrationBuilder.CreateIndex("IX_REDEEM_TUTOR_ID",        "REDEEM",           "TUTOR_ID");
            migrationBuilder.CreateIndex("IX_REDEEM_COUPON_ID",       "REDEEM",           "COUPON_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop na ordem inversa (filhos antes dos pais)
            migrationBuilder.DropTable("REDEEM");
            migrationBuilder.DropTable("COUPON");
            migrationBuilder.DropTable("COUPON_TEMPLATE");
            migrationBuilder.DropTable("PARTNER_DISCOUNT");
            migrationBuilder.DropTable("REWARD_POINT");
            migrationBuilder.DropTable("RISK_SCORE");
            migrationBuilder.DropTable("SUBSCRIPTION");
            migrationBuilder.DropTable("HEALTH_EVENT");
            migrationBuilder.DropTable("ADDRESS");
            migrationBuilder.DropTable("PET");
            migrationBuilder.DropTable("PLAN");
            migrationBuilder.DropTable("CLINIC");
            migrationBuilder.DropTable("TUTOR");
            migrationBuilder.DropTable("REWARD_ACTION");
            migrationBuilder.DropTable("RISK_LEVEL");
            migrationBuilder.DropTable("EVENT_TYPE");
            migrationBuilder.DropTable("SPECIES");
        }
    }
}

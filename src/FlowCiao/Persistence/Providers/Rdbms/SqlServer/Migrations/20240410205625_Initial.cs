using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "FlowCiao");

            migrationBuilder.CreateTable(
                name: "Activity",
                schema: "FlowCiao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityType = table.Column<int>(type: "int", nullable: false),
                    ActorName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ActorContent = table.Column<byte[]>(type: "varbinary(max)", maxLength: 1000000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flow",
                schema: "FlowCiao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flow", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlowInstance",
                schema: "FlowCiao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExecutionState = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Progress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowInstance_Flow_FlowId",
                        column: x => x.FlowId,
                        principalSchema: "FlowCiao",
                        principalTable: "Flow",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "State",
                schema: "FlowCiao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsFinal = table.Column<bool>(type: "bit", nullable: false),
                    IsInitial = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                    table.ForeignKey(
                        name: "FK_State_Flow_FlowId",
                        column: x => x.FlowId,
                        principalSchema: "FlowCiao",
                        principalTable: "Flow",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StateActivity",
                schema: "FlowCiao",
                columns: table => new
                {
                    StateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateActivity", x => new { x.StateId, x.ActivityId });
                    table.ForeignKey(
                        name: "FK_StateActivity_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "FlowCiao",
                        principalTable: "Activity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StateActivity_State_StateId",
                        column: x => x.StateId,
                        principalSchema: "FlowCiao",
                        principalTable: "State",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transition",
                schema: "FlowCiao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transition_Flow_FlowId",
                        column: x => x.FlowId,
                        principalSchema: "FlowCiao",
                        principalTable: "Flow",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transition_State_FromId",
                        column: x => x.FromId,
                        principalSchema: "FlowCiao",
                        principalTable: "State",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transition_State_ToId",
                        column: x => x.ToId,
                        principalSchema: "FlowCiao",
                        principalTable: "State",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransitionActivity",
                schema: "FlowCiao",
                columns: table => new
                {
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransitionActivity", x => new { x.TransitionId, x.ActivityId });
                    table.ForeignKey(
                        name: "FK_TransitionActivity_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "FlowCiao",
                        principalTable: "Activity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransitionActivity_Transition_TransitionId",
                        column: x => x.TransitionId,
                        principalSchema: "FlowCiao",
                        principalTable: "Transition",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Trigger",
                schema: "FlowCiao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TriggerType = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    TransitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trigger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trigger_Transition_TransitionId",
                        column: x => x.TransitionId,
                        principalSchema: "FlowCiao",
                        principalTable: "Transition",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ActorName",
                schema: "FlowCiao",
                table: "Activity",
                column: "ActorName",
                unique: true,
                filter: "[ActorName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstance_FlowId",
                schema: "FlowCiao",
                table: "FlowInstance",
                column: "FlowId");

            migrationBuilder.CreateIndex(
                name: "IX_State_FlowId",
                schema: "FlowCiao",
                table: "State",
                column: "FlowId");

            migrationBuilder.CreateIndex(
                name: "IX_StateActivity_ActivityId",
                schema: "FlowCiao",
                table: "StateActivity",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Transition_FlowId",
                schema: "FlowCiao",
                table: "Transition",
                column: "FlowId");

            migrationBuilder.CreateIndex(
                name: "IX_Transition_FromId",
                schema: "FlowCiao",
                table: "Transition",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Transition_ToId",
                schema: "FlowCiao",
                table: "Transition",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_TransitionActivity_ActivityId",
                schema: "FlowCiao",
                table: "TransitionActivity",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Trigger_TransitionId",
                schema: "FlowCiao",
                table: "Trigger",
                column: "TransitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlowInstance",
                schema: "FlowCiao");

            migrationBuilder.DropTable(
                name: "StateActivity",
                schema: "FlowCiao");

            migrationBuilder.DropTable(
                name: "TransitionActivity",
                schema: "FlowCiao");

            migrationBuilder.DropTable(
                name: "Trigger",
                schema: "FlowCiao");

            migrationBuilder.DropTable(
                name: "Activity",
                schema: "FlowCiao");

            migrationBuilder.DropTable(
                name: "Transition",
                schema: "FlowCiao");

            migrationBuilder.DropTable(
                name: "State",
                schema: "FlowCiao");

            migrationBuilder.DropTable(
                name: "Flow",
                schema: "FlowCiao");
        }
    }
}

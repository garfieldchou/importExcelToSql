﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SteamData;

namespace importSteamToSql.Migrations
{
    [DbContext(typeof(SteamDataContext))]
    [Migration("20200827065214_ChangeAvgAndMaxToDecimal")]
    partial class ChangeAvgAndMaxToDecimal
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SteamData.DownloadedStatistics.CountryDLStatOverview", b =>
                {
                    b.Property<int>("CountryDLStatOverviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("AvgDlSpeedMbps")
                        .HasColumnType("decimal(18,3)");

                    b.Property<int>("CountryListId")
                        .HasColumnType("int");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<decimal>("SteamPercent")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalTb")
                        .HasColumnType("decimal(18,9)");

                    b.Property<int>("WorkWeek")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("CountryDLStatOverviewId");

                    b.HasIndex("CountryListId");

                    b.ToTable("CountryDLStatOverviews");
                });

            modelBuilder.Entity("SteamData.DownloadedStatistics.CountryList", b =>
                {
                    b.Property<int>("CountryListId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Territory")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryListId");

                    b.HasIndex("Country")
                        .IsUnique()
                        .HasFilter("[Country] IS NOT NULL");

                    b.ToTable("CountryLists");
                });

            modelBuilder.Entity("SteamData.DownloadedStatistics.CountryNetworkDLStat", b =>
                {
                    b.Property<int>("CountryNetworkDLStatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("AvgDlSpeedMbps")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("CountryListId")
                        .HasColumnType("int");

                    b.Property<string>("Network")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("CountryNetworkDLStatId");

                    b.HasIndex("CountryListId");

                    b.ToTable("CountryNetworkDLStats");
                });

            modelBuilder.Entity("SteamData.DownloadedStatistics.RegionDLStatDetail", b =>
                {
                    b.Property<int>("RegionDLStatDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("BandWidthGbps")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<DateTime>("Full_DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.Property<int>("WorkWeek")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("RegionDLStatDetailId");

                    b.ToTable("RegionDLStatDetails");
                });

            modelBuilder.Entity("SteamData.DownloadedStatistics.RegionDLStatOverview", b =>
                {
                    b.Property<int>("RegionDLStatOverviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Average")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<decimal>("Max")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkWeek")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("RegionDLStatOverviewId");

                    b.ToTable("RegionDLStatOverviews");
                });

            modelBuilder.Entity("SteamData.GameRanks.DetailsGame", b =>
                {
                    b.Property<int>("DetailsGameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AllReviews")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Game")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GameDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HotTags")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Links")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecentReviews")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SystemRequirements")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DetailsGameId");

                    b.ToTable("DetailsGames");

                    b.HasData(
                        new
                        {
                            DetailsGameId = 1,
                            AllReviews = "",
                            Game = "",
                            GameDescription = "",
                            HotTags = "",
                            RecentReviews = "",
                            ReleaseDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SystemRequirements = ""
                        });
                });

            modelBuilder.Entity("SteamData.GameRanks.GameRank", b =>
                {
                    b.Property<int>("GameRankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<int>("DetailsGameId")
                        .HasColumnType("int");

                    b.Property<string>("Game")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Links")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<int>("Peak")
                        .HasColumnType("int");

                    b.Property<int>("Players")
                        .HasColumnType("int");

                    b.Property<int>("Ranks")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<int>("WorkWeek")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("GameRankId");

                    b.HasIndex("DetailsGameId");

                    b.ToTable("GameRanks");
                });

            modelBuilder.Entity("SteamData.GameRanks.OnlineStat", b =>
                {
                    b.Property<int>("OnlineStatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<int>("Players")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.Property<int>("WorkWeek")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("OnlineStatId");

                    b.ToTable("OnlineStats");
                });

            modelBuilder.Entity("SteamData.HardwareSoftwareSurvey.DirectXOS", b =>
                {
                    b.Property<int>("DirectXOSId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<string>("Item")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<int>("WorkWeek")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("DirectXOSId");

                    b.ToTable("DirectXOSs");
                });

            modelBuilder.Entity("SteamData.HardwareSoftwareSurvey.HWSurvey", b =>
                {
                    b.Property<int>("HWSurveyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<string>("Item")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<int>("WorkWeek")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("HWSurveyId");

                    b.ToTable("HWSurveys");
                });

            modelBuilder.Entity("SteamData.HardwareSoftwareSurvey.PCVideoCardUsageDetail", b =>
                {
                    b.Property<int>("PCVideoCardUsageDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<string>("Item")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<int>("WorkWeek")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("PCVideoCardUsageDetailId");

                    b.ToTable("PCVideoCardUsageDetails");
                });

            modelBuilder.Entity("SteamData.HardwareSoftwareSurvey.PcPhyCpuDetail", b =>
                {
                    b.Property<int>("PcPhyCpuDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<string>("Item")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<int>("WorkWeek")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("PcPhyCpuDetailId");

                    b.ToTable("PcPhyCpuDetails");
                });

            modelBuilder.Entity("SteamData.HardwareSoftwareSurvey.ProceUsageDetail", b =>
                {
                    b.Property<int>("ProceUsageDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<string>("Item")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<int>("WorkWeek")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("ProceUsageDetailId");

                    b.ToTable("ProceUsageDetails");
                });

            modelBuilder.Entity("SteamData.DownloadedStatistics.CountryDLStatOverview", b =>
                {
                    b.HasOne("SteamData.DownloadedStatistics.CountryList", "CountryList")
                        .WithMany()
                        .HasForeignKey("CountryListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SteamData.DownloadedStatistics.CountryNetworkDLStat", b =>
                {
                    b.HasOne("SteamData.DownloadedStatistics.CountryList", "CountryList")
                        .WithMany()
                        .HasForeignKey("CountryListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SteamData.GameRanks.GameRank", b =>
                {
                    b.HasOne("SteamData.GameRanks.DetailsGame", "DetailsGame")
                        .WithMany()
                        .HasForeignKey("DetailsGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

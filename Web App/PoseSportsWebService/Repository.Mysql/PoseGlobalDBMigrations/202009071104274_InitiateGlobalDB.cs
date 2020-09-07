﻿namespace Repository.Mysql.PoseGlobalDBMigrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitiateGlobalDB : DbMigration
    {
        public override void Up()
        {
            Sql(@"create table `in_app_billing` (`trans_no` bigint not null  auto_increment ,`user_no` bigint not null ,`store_type` varchar(16)  not null ,`product_id` varchar(48)  not null ,`order_id` varchar(64) ,`purchase_token` varchar(1024) ,`purchase_state` varchar(16)  not null ,`upt_date` datetime not null ,`ipt_date` datetime not null ,primary key ( `trans_no`) ) engine=InnoDb auto_increment=0
CREATE index  `IDX_USER_NO` on `in_app_billing` (`user_no` ) using HASH
CREATE index  `IDX_PRODUCT_ID` on `in_app_billing` (`product_id` ) using HASH
CREATE index  `IDX_ORDER_ID` on `in_app_billing` (`order_id` ) using HASH
create table `user_base` (`user_no` bigint not null  auto_increment ,`platform_id` varchar(128)  not null ,`platform_type` varchar(16)  not null ,`platform_email` varchar(128) ,`last_login_date` datetime not null ,`ipt_date` datetime not null ,primary key ( `user_no`) ) engine=InnoDb auto_increment=0
CREATE UNIQUE index  `IDX_PLATFORM_ID` on `user_base` (`platform_id` ) using HASH
CREATE index  `IDX_PLATFORM_EMAIL` on `user_base` (`platform_email` ) using HASH
create table `user_role` (`user_no` bigint not null ,`role_type` varchar(16)  not null ,`linked_trans_no` bigint not null ,`expire_time` datetime not null ,`upt_date` datetime not null ,primary key ( `user_no`) ) engine=InnoDb auto_increment=0
create table `__MigrationHistory` (`MigrationId` nvarchar(150)  not null ,`ContextKey` nvarchar(300)  not null ,`Model` longblob not null ,`ProductVersion` nvarchar(32)  not null ,primary key ( `MigrationId`) ) engine=InnoDb auto_increment=0
INSERT INTO `__MigrationHistory`(
`MigrationId`,
`ContextKey`,
`Model`,
`ProductVersion`) VALUES (
'202009071104274_InitiateGlobalDB',
'Repository.Mysql.PoseGlobalDBMigrations.Configuration',
0x1F8B0800000000000400E55ADB6EDB46107D2FD07F20F8EC88B61304A92125B0253B106A59862E41DF8415B9921759EE32DCA52121E897F5A19FD45FE82CEF77931255372DFC42EDE5CC6567666766FDD71F7FF63FED6CAA3D635710CE06FA45EF5CD73033B945D876A07B72F3E683FEE9E3CF3FF56F2D7BA77D89D6BD55EB60271303FD494AE7CA3084F9846D247A36315D2EF846F64C6E1BC8E2C6E5F9F92FC6C585810142072C4DEBCF3C26898DFD1FF073C899891DE9213AE116A6221C8799B98FAA3D201B0B079978A0CFB0C30591DCDDF7267BF18DF660B3C43B2974ED9A12040CCD31DDE81A628C4B2481DDABA5C073E972B69D3B3080E862EF6058B74154E0508CAB64795389CE2F954446B23182323D21B9DD12F0E26DA82223BFFD2045EBB10A4189B7A06CB95752FB8A1CE86376ED3837845238655DCB13BC1A52572D2ED1F52317F833E56B444737BD055A532C7A69B0332DBFE52C361AB02DF577A60D3D2A3D170F18F6A48B60C5A3B7A6C4FC15EF17FC2B6603E6519AE61F2480B9CC000C3DBADCC1AEDCCFF026940AC0985831AE6B4676B791DF1E6F2EEC0CE41E33F9FE9DAE3D00234AC6D854523A9A8388F83366D845125B8F484AECC2498F2DEC2BBBC0438EA227B0DB80603D88503CAC2420443860E6FE914ED0EE1EB3AD7C02977EAF6B776487AD6820845E32026EEE0BEFE1D6A41D975B9E2957C4AA21FDEEC3294873D702DDD51256BA6C40F805113DD77C420214ACACB24EC3E7979DD2136062FFFC897A8E5C5929C223F85E408C6E0D440E03EA1B4998AA0D5E10CDDD1BA4B61F19B822A0570D5A7120681BB31A4690EE42964391DC70D7AE77BD8BCB93387D4CFC75A25D441D2E5E423B90BE9E1C4542AE28DF12F6E378E48CD36E3C5201FDCF3DF281B317CFD40535BD8E33409EF7155BABA639533D18DE3944E530BE351E69E607DE60F566AEAA0B44E084422269B32DB177B0E0D0E445A8DE2CCB01E81CCBC835339978C24850C5F4B289BA518FE5C517630127B9331B6004AE5C8A114C95692FD65352BB1941F1161579464595D79F20C701F952555F38A2CD83926FF866DEBE08B2030CC31425B550CC6D4C093C116D716E164803A777C415128C09AD7D0D0E2DBBB02C6315151A8E48951C7C3E2825BA8F36A9EF606365FD9B35961C64A2D43B90D3861BDF1719C7DCE58AC2C276BF164714B995A5D39053CF66F5A5581D521C3ED34015C1B80E275D15A5A1D2E3CDD1D2854E1A2D3DDE1C2DA95DD258C9680BBE72D54986B7DCDC01A8610D528A1ACEB538D93830678E361E6D8E444A91480D52DFC8D97DDED78C82B3E5AEFDBC033772EF381677E3DACB0AB8066E5DBDF5D48E9829183296949E3800AFE8DBB9A90330C3CCBE14349C6B8E5A48E0D3B085C9FFA8F9FBB94277E65F06D7D0FCCBB79EDAFC53197A1A2935DCC2A0F23977C6A0F293CD7133E9771A333371CA487F52032D24A7F92531F53849CD25A3FD30317CF95DA29029064B740D54F44C2C95254EF673B06A35DFF33F879480C0C98A09626483850CEA57FD837A64C9BC69FC7BDE170C212CDAE291E155DBF96B02C116F4FC6203AC6DC1992DF8233285A270CC2CBC1BE8DFFD6D57DA78F4DB6A39BF9DAD1EA667DA54E57E57DAC599361650977FF360C19D22AEFDDEC17BC03352399B5BE80A1CDDED8F819B08FB389B8E96C3C56A3C7A41DEDCDBC1910F03AD789CCE4670226D38CC763B0E7F51283D23FF49E140F4CCFB41471690EFADA8EFE0727874B1498257E1F3A33B9307C1B67F3978C5AEE0A962514977BE9D8FDE5F2FEEA6B349A50B2CC004731EE077BBBB68E47715A64ADBF487A9E176723DBE6F1E0C72AA38A2CBFFC37A5631CB7E1DCF6AA9A84223BD2363AC68931FC865499FBC2343E924B2B7EBA1179BAAF54DE9719B26799076831C6BA5F38063703148EB57EB868DF46593467A191DDF22D74DFAECCB267DF64A12EE316DF86245D337D2FF8FD51F6141B60984FAEF2C864D153113D068CD986D786450205D9AA36849CEDE265822B03174ED4AB241A68469130BE19FCB17443D58726BAFB13566534F3A9EBC1602DB6B9A799EEE1BF5F4FDB7862CCFFDA9A37E892E44003689729329BBF108B562BEEF4A1CA30242B94078FB035773A9B280ED3E462ABE00560185EA1B610733953B2CB0ADAE412CA66C8E9EF121BC8101DEE32D32F7515D5A0DF2F24164D5DE1F11B475912D428C643FFC041BB6ECDDC7BF0184A4F01F96280000,
'6.4.0');");
        }

        public override void Down()
        {
            DropIndex("user_base", "IDX_PLATFORM_EMAIL");
            DropIndex("user_base", "IDX_PLATFORM_ID");
            DropIndex("in_app_billing", "IDX_ORDER_ID");
            DropIndex("in_app_billing", "IDX_PRODUCT_ID");
            DropIndex("in_app_billing", "IDX_USER_NO");
            DropTable("user_role");
            DropTable("user_base");
            DropTable("in_app_billing");
        }
    }
}
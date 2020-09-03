USE `pose_globaldb`;
DROP procedure IF EXISTS `P_INSERT_USER_BASE`;

DELIMITER $$
USE `pose_globaldb`$$
CREATE DEFINER=`poseadmin`@`%` PROCEDURE `P_INSERT_USER_BASE`(
	IN	i_platform_id		VARCHAR(128),
	IN	i_platform_type		VARCHAR(16),
    IN  i_platform_email    VARCHAR(64),
    IN	i_role_type		    VARCHAR(16),
    IN  i_cur_time          DATETIME,
	OUT o_result			INT
)
proc_body:
BEGIN
	DECLARE v_user_no BIGINT DEFAULT 0;

	DECLARE EXIT HANDLER FOR SQLEXCEPTION
		BEGIN
			SET o_result = 101;
		END;

	SET o_result = 1;
    
    IF EXISTS (SELECT user_no FROM user_base WHERE platform_id = i_platform_id)
	THEN    
		SET o_result = 0;
		leave proc_body;
	END IF;
        
    BEGIN
        DECLARE EXIT HANDLER FOR SQLEXCEPTION
			BEGIN
				ROLLBACK;
				SET o_result = 102;
            END;      	        
		START TRANSACTION;
            INSERT INTO user_base
            (
                platform_id,
                platform_type,
                platform_email,
                last_login_date,
                ipt_date
            )
            VALUES
            (
                i_platform_id,
                i_platform_type,
                i_platform_email,
                i_cur_time,
                i_cur_time
            );

            SET v_user_no = LAST_INSERT_ID();

            INSERT INTO user_role
            (
                user_no,
                role_type,
                linked_trans_no,
                expire_time,
                upt_date
            )
            VALUES
            (
                v_user_no,
                i_role_type,
                0,
                i_cur_time,
                i_cur_time
            );
            
            SET o_result = 0;
        COMMIT;       
	END;    
END$$

DELIMITER ;


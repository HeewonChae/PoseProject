USE `pose_globaldb`;
DROP procedure IF EXISTS `P_UPDATE_IN_APP_BILLING`;

DELIMITER $$
USE `pose_globaldb`$$
CREATE DEFINER=`poseadmin`@`%` PROCEDURE `P_UPDATE_IN_APP_BILLING`(
	IN	i_user_no		    BIGINT,
	IN	i_trans_no		    BIGINT,
    IN  i_order_id          VARCHAR(64),
    IN	i_purchase_token    VARCHAR(1024),
    IN  i_purchase_state    VARCHAR(16),
    IN  i_role_type         VARCHAR(16),
    IN  i_role_expire       DATETIME,
    IN  i_cur_time          DATETIME,

	OUT o_result			INT
)
proc_body:
BEGIN
	DECLARE EXIT HANDLER FOR SQLEXCEPTION
		BEGIN
			SET o_result = 101;
		END;

	SET o_result = 1;

    IF NOT EXISTS (SELECT trans_no FROM in_app_billing WHERE trans_no = i_trans_no)
	THEN    
		SET o_result = 2;
		leave proc_body;
	END IF;
    
    START TRANSACTION;
        UPDATE in_app_billing
        SET 
            order_id = i_order_id
        ,   purchase_token = i_purchase_token
        ,   purchase_state = i_purchase_state
        ,   upt_date = i_cur_time
        WHERE
            trans_no = i_trans_no
        AND user_no = i_user_no;

        UPDATE user_role
        SET
            role_type = i_role_type
        ,   linked_trans_no = i_trans_no
        ,   expire_time = i_role_expire
        ,   upt_date = i_cur_time
        WHERE
            user_no = i_user_no;

        IF ROW_COUNT() = 0 THEN
            SET o_result = 3;
            leave proc_body;
        END IF;
        
        SET o_result = 0;
    COMMIT;       
END$$

DELIMITER ;


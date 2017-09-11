delimiter $$
CREATE TRIGGER `after_policyinbox_insert` AFTER INSERT ON `policyinbox`
 FOR EACH ROW 
 BEGIN
	INSERT INTO customer_profile values(null,
	(SELECT new.source_phone FROM POLICYINBOX LIMIT 1),
	SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',-7),'*',1)
    ,SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',-6),'*',1)
    ,SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',-6),'*',1)
    ,SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',-5),'*',1)
    ,'Unposted','20170801' ) ON DUPLICATE KEY			
    UPDATE 
	email = SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',-7),'*',1)
    , lastname=SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',-6),'*',1)
    ,othername=SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',-6),'*',1)
    ,address=SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',-5),'*',1);
END;
delimiter ;


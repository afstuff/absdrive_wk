DROP TRIGGER IF EXISTS `after_policyinbox_insert`;
delimiter $$
CREATE TRIGGER `after_policyinbox_insert` AFTER INSERT ON `policyinbox`
 FOR EACH ROW 
 BEGIN

insert into customer_profile
VALUES(
null
,new.source_phone
,SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',3),'*',-1)
,SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',4),'*',-1)
,SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',5),'*',-1)
,SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',6),'*',-1)
,'Unposted'
,now() 
) on duplicate KEY
UPDATE 
email = SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',3),'*',-1)
,address = SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',6),'*',-1)
,lastname = SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',4),'*',-1)
,othername = SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',5),'*',-1);

INSERT INTO customer_transactions VALUES(
null
,new.source_phone
,SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',3),'*',-1)
,'Third Party'
,'policy number'
, now()
, 5000
,1000000
,SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',-1),'#',1)
,SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',7),'*',-1)
,SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',9),'*',-1)
,SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',10),'*',-1)
,SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',12),'*',-1)
,'20170301'
,SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',8),'*',-1)
,' '
,SUBSTRING_INDEX(SUBSTRING_INDEX(new.message,'*',11),'*',-1)
,'pending'
,'Admin'
,now()
);



END$$
delimiter ;

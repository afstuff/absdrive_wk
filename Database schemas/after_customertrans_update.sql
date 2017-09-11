DROP TRIGGER IF EXISTS `after_customertrans_update`;
delimiter $$
CREATE TRIGGER `after_customertrans_update` AFTER UPDATE ON `customer_transactions` 
FOR EACH ROW 
BEGIN
	/*
    insert into the niid_motordetails_online table
    */
    INSERT INTO niid_motordetails_online 
     select null,
	'username',
	'password'
    ,' ' 
    ,new.policyno
    ,(select concat(a.lastname, ' ',  a.othername) from customer_profile a where a.Email = new.email )
    ,(select a.Address from customer_profile a where a.Email = new.email )
    ,new.phoneno
    ,new.email
    ,new.fld_4
    ,new.fld_5
    ,new.product
    ,' '
    ,new.fld_6
    ,new.fld_7
    ,' '
    ,new.fld_8
    ,new.fld_2
    ,new.fld_1
    ,new.fld_3
    ,' '
    ,' '
    ,new.suminsured
    ,new.amount
    ,' '
    ,' '
    ,' '
    ,'U'
    ,' '
    ,now()
    ,now()
  where new.policyno <> old.policyno;                                                
END$$
delimiter ;
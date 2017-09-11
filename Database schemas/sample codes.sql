select @message = '*2*jcc_nnannah@yahoo.co.uk*Sanders*Jamello *43 Islander McKenna Blvd. Victoria Island *DFG 657 MK*56568837477*TOYOTA*SALOON*2013*20170301*4665321246786 67774432254 437774333356#';
BEGIN

insert into customer_profile
values(
NULL
,1
,2
,3
,4
,5,'Unposted'
,now() ) ON DUPLICATE KEY
UPDATE 
 email = 'afdhkahdksk'
,address = SUBSTRING_INDEX(SUBSTRING_INDEX(@message,'*',6),'*',-1)
,lastname = SUBSTRING_INDEX(SUBSTRING_INDEX(@message,'*',4),'*',-1)
,othername = SUBSTRING_INDEX(SUBSTRING_INDEX(@message,'*',5),'*',-1);



/* insert into the customer trans table*/
INSERT INTO customer_transactions VALUES(
null
,'334433333'
,SUBSTRING_INDEX(SUBSTRING_INDEX(@message,'*',3),'*',-1)
,'Third Party'
,'policy number'
, now()
, 5000
,1000000
,SUBSTRING_INDEX(SUBSTRING_INDEX(@message,'*',-1),'#',1)
,SUBSTRING_INDEX(SUBSTRING_INDEX(@message,'*',7),'*',-1)
,SUBSTRING_INDEX(SUBSTRING_INDEX(@message,'*',9),'*',-1)
,SUBSTRING_INDEX(SUBSTRING_INDEX(@message,'*',10),'*',-1)
,SUBSTRING_INDEX(SUBSTRING_INDEX(@message,'*',12),'*',-1)
,'20170301'
,SUBSTRING_INDEX(SUBSTRING_INDEX(@message,'*',8),'*',-1)
,' '
,SUBSTRING_INDEX(SUBSTRING_INDEX(@message,'*',11),'*',-1)
,'pending'
,'Admin'
,now()
);

END
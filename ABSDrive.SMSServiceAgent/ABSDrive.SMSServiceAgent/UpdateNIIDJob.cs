using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using UpdateNIID.Data;
using UpdateNIID.Model;
using UpdateNIID.Repositories;

namespace ABSDrive.SMSServiceAgent
{
    public class UpdateNIIDJob:IJob
    {
        private UpdateNIID.Data.MotorDetailsRepository absData;
        private UpdateNIID.Data.Service req = new Service();
        Int64 cnt = 0;
        string m;
        string returnvalue;

        //empty constructor
        public UpdateNIIDJob()
        {

        }
        public void Execute(IJobExecutionContext context)
        {

            absData = new MotorDetailsRepository();
            MotorDetailsOnline mdo = new MotorDetailsOnline();


            IList<MotorDetailsOnline> MotorData = absData.MotorDetails();
            try
            {
                ActivityLog.ErrorLog("NIID Update about to begin...", "SMSapplication_ErrorLog_");
                foreach (MotorDetailsOnline md in MotorData)
                {
                    if (md.Status != "P")
                    {
                        returnvalue = req.Vehicle_Policy_Push(md.Username, md.Password, md.NiaNaicomID, md.PolicyNo,
                          md.InsuredName, md.ContactAddress, md.GSMNo, md.Email, md.EffectiveCoverDate, md.ExpirationDate, md.TypeOfCover, md.VehicleCategory,
                          md.EngineNo, md.ChasisNo, md.VehicleColor, md.YearofMake, md.VehicleMake, md.RegistrationNo, "", md.VehicleType, md.EngineCapacity,
                          md.VehicleModel, md.SumAssured, md.Premium, md.CoverNoteNo, md.CertificateNo, md.GeographicalZone);


                        if (returnvalue == "Active Policy in force!")
                        {
                            cnt = cnt + 1;
                            md.Status = "P";
                            md.ProcessDate = DateTime.Now;
                        }
                        md.ReturnMessage = returnvalue;
                        absData.Save(md);
                        mdo = (MotorDetailsOnline)md;

                    }

                }
                m = cnt.ToString() + " records out of " + MotorData.Count.ToString() + " uploaded unto NIID";
                ActivityLog.ErrorLog(m, "SMSapplication_ErrorLog_");


            }
            catch (Exception x)
            {
                ActivityLog.ErrorLog("Error!: " + x.Message, "SMSapplication_ErrorLog_");
                mdo.ReturnMessage = x.Message;
                absData.Save(mdo);

            }
         }

      }
    }



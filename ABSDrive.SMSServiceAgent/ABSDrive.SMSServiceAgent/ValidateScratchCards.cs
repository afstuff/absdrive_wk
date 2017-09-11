using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABSDrive.Repositories;

namespace ABSDrive.SMSServiceAgent
{
    public class ValidateScratchCards:IPaymentValidator
    {
       public ValidateScratchCards()
        {

        }
        /// <summary>
        /// Retrieves the value of the Scratch Card(s) from the Gateway 
        /// </summary>
        /// <param name="cards">A string array of values of the cards</param>
        /// <returns>the total amount of the cards sent into the function</returns>
        public decimal paymentGateway(String[] codeValues){
            Decimal totCardValue = 0;
            foreach (var card in codeValues)
            {
                totCardValue  =+  getCardValue(card);
                
            }

            return totCardValue;

        }

        private decimal getCardValue(string cardInfo)
        {
            
            IDictionary<String,Decimal> cardValues = new Dictionary<String,Decimal>();
            cardValues.Add("444i6502DD5949", 1000);
            cardValues.Add("75795983845985", 2000);
            
            var totValue =cardValues.Sum(c=>c.Value);


            return totValue;
        }
    }
}

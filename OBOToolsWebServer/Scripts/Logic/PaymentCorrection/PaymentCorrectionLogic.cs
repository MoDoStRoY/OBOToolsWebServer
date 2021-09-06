using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OBOToolsWebServer.Scripts.Logic.PaymentCorrection
{
    public class PaymentCorrectionLogic
    {
        public static string TTNumber;
        public static string contact;
        public static string BCVariantCLB;
        public static string correctNumber;
        public static string incorrectNumber;
        public static double paymentSum;
        public static string paymentDate;
        public static double correctionSum;
        public static string sourceTicket;

        public static bool refusedCorrectionCB;
        public static bool incorrectTicket;
        public static bool fullCorrectionCB;
        public static bool reparationCB;

        public static string decision;
        public static string kassaComment;
        public static string reparationComment;

        public static List<string> answerOnRequest;


        public static List<string> GetResults(List<string> dataListIn)
        {
            PushDataInVars(dataListIn);

            NormalizeAll();

            GetDecision();
            GetKassaComment();
            GetInvoiceComment();

            FillAnswer();

            return answerOnRequest;
        }

        private static void PushDataInVars(List<string> dataListIn)
        {
            TTNumber = dataListIn[0];
            contact = dataListIn[1];
            BCVariantCLB = dataListIn[2];
            correctNumber = dataListIn[3];
            incorrectNumber = dataListIn[4];
            paymentSum = Convert.ToDouble(dataListIn[5]);
            paymentDate = dataListIn[6];
            correctionSum = Convert.ToDouble(dataListIn[7]);
            sourceTicket = dataListIn[8];
            refusedCorrectionCB = NormalizeStrings.Bool(dataListIn[9]);
            incorrectTicket = NormalizeStrings.Bool(dataListIn[10]);
            fullCorrectionCB = NormalizeStrings.Bool(dataListIn[11]);
            reparationCB = NormalizeStrings.Bool(dataListIn[12]);
        }

        private static void NormalizeAll()
        {
            TTNumber = NormalizeStrings.TTNumber(TTNumber);
            contact = NormalizeStrings.Number(contact);
            BCVariantCLB = NormalizeStrings.BCVariant();
            correctNumber = NormalizeStrings.Number(correctNumber);
            incorrectNumber = NormalizeStrings.Number(incorrectNumber);
            paymentSum = NormalizeStrings.Sum(paymentSum.ToString());
            paymentDate = NormalizeStrings.Date(paymentDate);
            correctionSum = NormalizeStrings.Sum(correctionSum.ToString());
            sourceTicket = NormalizeStrings.SourceTicket();
        }

        private static void GetDecision()
        {
            string decisionLocal = "OBO Tech: ";

            if (refusedCorrectionCB)
            {
                decisionLocal += "В корректировке отказано - ХХХХХХХХХХХХХХ" + "\n\n" + "===============================\n\n";
            }
            else
            {
                if (fullCorrectionCB)
                {
                    decisionLocal += "Платёж скорректирован в полном объёме - " + paymentSum +
                        " руб. \n\n===============================\n\n";
                }
                else
                {
                    decisionLocal += "Платёж скорректирован частично - " + correctionSum +
                        " руб. \n\n===============================\n\n";
                }
            }

            if (incorrectTicket)
            {
                decisionLocal += "Решение на первой линии:\n";

                if (sourceTicket.Equals("CRM"))
                {
                    decisionLocal += "https://kms.tele2.ru/kms/CM/SCENARIO/VIEW?item_id=22955";
                }
                else
                {
                    decisionLocal += "https://kms.tele2.ru/kms/CM/SCENARIO/VIEW?item_id=1175897";
                }

                decisionLocal += "\n\nПричинаОшибки\n\n";
            }

            if (BCVariantCLB.Equals("Звонок"))
            {
                decisionLocal += "Информация предоставлена.\n\n" + "Способ ОС: звонком на номер " + contact;

                if (reparationCB)
                {
                    decisionLocal += "\nКомпенсация: " + NormalizeStrings.Difference(paymentSum, correctionSum) +
                        " руб. на номер " + correctNumber;
                }
            }
            else
            {
                decisionLocal += "Способ ОС: " + BCVariantCLB + " на номер " + contact;

                if (reparationCB)
                {
                    decisionLocal += "\nКомпенсация: " + NormalizeStrings.Difference(paymentSum, correctionSum) +
                        " руб. на номер " + correctNumber + "\n\n";
                }
                else
                {
                    decisionLocal += "\n\n";
                }

                decisionLocal += GetAnswer();
            }
            decision = decisionLocal;
        }

        private static string GetAnswer()
        {
            string answer = "Здравствуйте, меня зовут София, я занималась рассмотрением Вашей заявки " + TTNumber + ". ";

            if (refusedCorrectionCB)
            {
                answer += "К сожалению, выполнить корректировку данного платежа на текущий момент невозможно, т.к. ХХХХХХХХХХХХХХ. " +
                    "Приносим извинения за доставленные неудобства. Надеюсь на Ваше понимание и благодарю за обращение!";
            }
            else
            {
                if (fullCorrectionCB)
                {
                    answer += "Сообщаю, что на номер +" + correctNumber + " перенесён ошибочный платёж на сумму "
                        + paymentSum + " руб. " + "Благодарю за обращение!";
                }
                else
                {
                    if (reparationCB)
                    {
                        answer += "Сообщаю, что платеж поступил на ошибочный номер, и абонент израсходовал часть средств. " +
                            "На номер +" + correctNumber + " перенесен остаток денежных средств в сумме " +
                            correctionSum + " руб. с ошибочного номера. Мы ценим Вас, поэтому дополнительно компенсировали " +
                            NormalizeStrings.Difference(paymentSum, correctionSum) +
                            " руб. В итоге, на Ваш номер возвращено " + paymentSum + " руб. Благодарю за обращение!";
                    }
                    else
                    {
                        answer += "Сообщаю, что платеж поступил на ошибочный номер, и абонент израсходовал часть средств. " +
                            "На номер +" + correctNumber + " перенесен остаток денежных средств в сумме " +
                            correctionSum + " руб. с ошибочного номера. " + "Надеюсь на Ваше понимание и благодарю за обращение!";
                    }
                }
            }

            return answer;
        }

        private static void GetKassaComment()
        {
            string comment = "Корректировка платежа на сумму " + paymentSum + " руб. от " + paymentDate +
                " с номера " + incorrectNumber + " на номер " + correctNumber + ". Всего скорректировано ";

            if (fullCorrectionCB)
            {
                comment += paymentSum + " руб. " + TTNumber;
            }
            else
            {
                comment += correctionSum + " руб. " + TTNumber;
            }

            kassaComment = comment;
        }

        private static void GetInvoiceComment()
        {
            string comment = "OBO Tech. " + TTNumber + " " + NormalizeStrings.Difference(paymentSum, correctionSum).ToString().Replace(',', '.');

            reparationComment = comment;
        }

        private static void FillAnswer()
        {
            answerOnRequest = new List<string>();

            answerOnRequest.Add(decision);
            answerOnRequest.Add(kassaComment);
            answerOnRequest.Add(reparationComment);
        }
    }
}

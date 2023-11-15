using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL.Controllers
{
    public class AddendaController : Controller
    {
        // GET: Addenda
        public ActionResult GetAll()
        {

            ML.Result result = BL.Addendas.GetAllLQ();
            ML.Addendas addendas = new ML.Addendas();
            addendas.ListAddendas = new List<object>();

            if (result.Correct)
            {
                addendas.ListAddendas = result.Objects;
            }
            else
            {
                ViewBag.Message = result.ErrorMessage;
            }
            return View(addendas);
        }


        [HttpGet]  // mostrar formulario
        public ActionResult Form(Guid? IdAddenda)
        {
            ML.Addendas addendas = new ML.Addendas();

            if (IdAddenda == null)
            {
                return View(addendas);
            }
            else
            {
                ML.Result result = BL.Addendas.GetByIdLQ(IdAddenda.Value);

                if (result.Correct)
                {
                    addendas.IdAddenda = ((ML.Addendas)result.Object).IdAddenda;
                    addendas.NombreAddenda = ((ML.Addendas)result.Object).NombreAddenda;
                    addendas.XML = ((ML.Addendas)result.Object).XML;
                    addendas.FechaModificacion = ((ML.Addendas)result.Object).FechaModificacion;
                    addendas.Usuario = ((ML.Addendas)result.Object).Usuario;
                    addendas.Estado = ((ML.Addendas)result.Object).Estado;
                }
                return View(addendas);
            }

        }

        [HttpPost] // recibe informacion del formulario update y add
        public ActionResult Form(ML.Addendas addendas)
        {

            if (addendas.IdAddenda == Guid.Empty)
            {
                ML.Result result = BL.Addendas.Add(addendas);

                if (result.Correct)

                {
                    ViewBag.Mensaje = "Se ha ingresado correctamente el registro";

                }
                else
                {
                    ViewBag.Mensaje = "No se ha ingresado correctamente el registro Error: " + result.ErrorMessage;
                }
            }
            else
            {
                ML.Result result = BL.Addendas.UpdateLQ(addendas);
                if (result.Correct)

                {
                    ViewBag.Mensaje = "Se ha actualizado correctamente el registro";

                }
                else
                {
                    ViewBag.Mensaje = "No se ha actualizado correctamente el registro Error: " + result.ErrorMessage;
                }
            }

            return PartialView("Modal");


        }



        public JsonResult UpdateStatus(Guid IdAddenda, bool Estado)
        {


            ML.Addendas addendas = new ML.Addendas();
            ML.Result result = BL.Addendas.GetByIdLQ(IdAddenda);

            if (result.Correct)
            {


                addendas = ((ML.Addendas)result.Object);
                addendas.Estado = (addendas.Estado.Value) ? false : true;

                ML.Result resultUpdate = BL.Addendas.UpdateLQ(addendas);

                ViewBag.Mensaje = "Se ha eliminado correctamente";
            }
            else
            {
                ViewBag.Mensaje = "No se ha eliminado correctamente";
            }
            return Json(result.Objects);
        }






        [HttpGet]  // mostrar formulario
        public ActionResult WebService()
        {
            PL.ServiceExamen.WsFactClient service = new ServiceExamen.WsFactClient();

            string base64XML = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4KPGNmZGk6Q29tcHJvYmFudGUgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSIgeG1sbnM6eHNkPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgVG90YWw9IjExNi4wMCIgRmVjaGE9IjIwMjItMTEtMjNUMTI6NDk6NTAiIEx1Z2FyRXhwZWRpY2lvbj0iMjYwMTUiIE1ldG9kb1BhZ289IlBQRCIgVGlwb0RlQ29tcHJvYmFudGU9IkkiIFRpcG9DYW1iaW89IjIwLjAwIiBNb25lZGE9IlVTRCIgQ2VydGlmaWNhZG89Ik1JSUZ1ekNDQTZPZ0F3SUJBZ0lVTXpBd01ERXdNREF3TURBME1EQXdNREkwTXpRd0RRWUpLb1pJaHZjTkFRRUxCUUF3Z2dFck1ROHdEUVlEVlFRRERBWkJReUJWUVZReExqQXNCZ05WQkFvTUpWTkZVbFpKUTBsUElFUkZJRUZFVFVsT1NWTlVVa0ZEU1U5T0lGUlNTVUpWVkVGU1NVRXhHakFZQmdOVkJBc01FVk5CVkMxSlJWTWdRWFYwYUc5eWFYUjVNU2d3SmdZSktvWklodmNOQVFrQkZobHZjMk5oY2k1dFlYSjBhVzVsZWtCellYUXVaMjlpTG0xNE1SMHdHd1lEVlFRSkRCUXpjbUVnWTJWeWNtRmtZU0JrWlNCallXUnBlakVPTUF3R0ExVUVFUXdGTURZek56QXhDekFKQmdOVkJBWVRBazFZTVJrd0Z3WURWUVFJREJCRFNWVkVRVVFnUkVVZ1RVVllTVU5QTVJFd0R3WURWUVFIREFoRFQxbFBRVU5CVGpFUk1BOEdBMVVFTFJNSU1pNDFMalF1TkRVeEpUQWpCZ2txaGtpRzl3MEJDUUlURm5KbGMzQnZibk5oWW14bE9pQkJRMFJOUVMxVFFWUXdIaGNOTVRrd05qRTNNVGswTkRFMFdoY05Nak13TmpFM01UazBOREUwV2pDQjRqRW5NQ1VHQTFVRUF4TWVSVk5EVlVWTVFTQkxSVTFRUlZJZ1ZWSkhRVlJGSUZOQklFUkZJRU5XTVNjd0pRWURWUVFwRXg1RlUwTlZSVXhCSUV0RlRWQkZVaUJWVWtkQlZFVWdVMEVnUkVVZ1ExWXhKekFsQmdOVkJBb1RIa1ZUUTFWRlRFRWdTMFZOVUVWU0lGVlNSMEZVUlNCVFFTQkVSU0JEVmpFbE1DTUdBMVVFTFJNY1JVdFZPVEF3TXpFM00wTTVJQzhnV0VsUlFqZzVNVEV4TmxGRk5ERWVNQndHQTFVRUJSTVZJQzhnV0VsUlFqZzVNVEV4TmsxSFVrMWFVakExTVI0d0hBWURWUVFMRXhWRmMyTjFaV3hoSUV0bGJYQmxjaUJWY21kaGRHVXdnZ0VpTUEwR0NTcUdTSWIzRFFFQkFRVUFBNElCRHdBd2dnRUtBb0lCQVFDTjBwZUtwZ2ZPTDc1aVlSdjFmcXErb1ZZc0xQVlVSL0dpYlltR0tjOUluSEZ5NWxZRjZPVFlqbklJdm1rT2RSb2JiR2xDVXhPUlgvdExzbDhZYTlnbTZZbzdoSG5PRFJCSUR1cDNHSVNGekIvOTZSOUsvTXpZUU9jc2NNSW9CREFSYXljbkx2eTdGbE12TzcvcmxWbnNTQVJ4WlJPOEt6OFpra3NqMnpwZVlwalpJeWEvMzY5K29HcVFrMWNUUmtIbzU5SnZKNFRmYmsvM2lJeWY0SC9Jbmk5bkJlOWNZV28wTW5Lb2I3RER0L3ZzZGk1dEE4bU10QTk1M0xhcE55Q1pJRENSUVFsVUdOZ0RxWTkvOEY1bVV2VmdrY2N6c0lnR2R2Zjl2TVFQU2YzampDaUtqN2o2dWN4bDErRndKV21idmdObWlhVVIvMHE0bTJybTc4bEZBZ01CQUFHakhUQWJNQXdHQTFVZEV3RUIvd1FDTUFBd0N3WURWUjBQQkFRREFnYkFNQTBHQ1NxR1NJYjNEUUVCQ3dVQUE0SUNBUUJjcGoxVGpUNGppaW5JdWpJZEFsRnpFNmtSd1lKQ25ERzA4elNwNGtTblNoanhBREdFWEgyY2hlaEtNVjBGWTdjNG5qQTVlREdkQS9HMk9DVFB2RjVycGVDWlA1RHc1MDRSWmtZRGwyc3VSeit3YTFzTkJWcGJuQkpFSzBmUWNOM0lmdEJ3c2dORmRGaFV0Q3l3M2x1czFTU0piUHhqTEhTNkZjWlo1MVlTZUlmY05YT0F1VHFkaW11c2FYcTE1R3JTckNPa002bjJqZmoyc01KWU0ySFhhWEo2ckdURWdZbWhZZHd4V3RpbDZSZlpCK2ZHUS9IOUk5V0xubDRLVFpVUzZDOStOTEhoNEZQRGhTazE5ZnBTMlMvNTZhcWdGb0dBa1hBWXQ5Rnk1RUNhUGNVTElmSjFERWJzWEt5UmRDdjNKWTg5KzBNTmtPZGFEbnNlbVMybzVHbDA4ekk0aVl0dDNMNDBnQVo2ME5QaDMxa1ZMbllOc212Zk54WXlLcCtBZUp0REh5Vzl3N2Z0TTBIb2krQnVSbWNBUVNLRlYzcGs4ajUxbGEranJSQnJBVXY4YmxiUmNRNUJpWlV3SnpIRkVLSXdUc1JHb1J5RXg5NnNObkIwM242R1R3aklHejkyU21MZE5sOTVyOXJrdnArMm00UzZxMWxQdVhhRmc3REdCclhXQzhpeXFlV0UyaW9iZHdJSXVYUFRNVnFRYjEybTFkQWtKVlJPNU5kSG5QL01wcU92T2dMcW9aQk5IR3lCZzRHcW00c0NKSEN4QTFjOEVsZmEyUlFUQ2swdEF6bGxMNHZPbkkxR0hrR0puNjV4b2tHc2FVNEI0RDM2eGg3ZVdyZmo0L3BnV0htdG9EQVlhOHd6U3dvMkdWQ1pPcyttdEVnT1FCOTEvZz09IiBOb0NlcnRpZmljYWRvPSIzMDAwMTAwMDAwMDQwMDAwMjQzNCIgRm9ybWFQYWdvPSIwMyIgU2VsbG89IlZkRlJ5U2ZUOUdIMm9EWU40YnJZZXpLcW90dVZoRXZRMGRMQVphV0QycTBrM2RqVDlnYlUxeExhaGZyNjM2Y1FzZC9rQjU5TEF4dW5wUDBnREg1cjRBMm1MRm5OcXFxZmpMNHZjalY1VmwyWEFBRGFKWUpvOFpDMVlxSXVaNjBaU3I5SmVtcEVvdGh0SzBXWDdDZWFjNXZ5L2JaaVgxUllvR1pUM3R1b2NNSVFwUDZ5R0dmMTZCMjdFN2J3RDFsdTJjZ2xQTlIzNHkxQ0xzZXVIczFVYmJtbEd4NGNwNVZUNEU2a3NGSU9QaWFNbWtBU2VUeXU1UkVOUG40S3FGZlM5SWdFTTdPNWNkTEVOQWZDYXdISzVLT2hjNjVEbWR6OVprK0VhNDl0RUlOSXBLSUExaW93TVJEQmFGZkNKTEEvdzBUdkV0Q1BabUN2ekhXbjBvc2hKdz09IiBGb2xpbz0iNTI2NCIgU2VyaWU9IkEiIFZlcnNpb249IjMuMyIgU3ViVG90YWw9IjEwMC4wMCIgeHNpOnNjaGVtYUxvY2F0aW9uPSJodHRwOi8vd3d3LnNhdC5nb2IubXgvY2ZkLzMgaHR0cDovL3d3dy5zYXQuZ29iLm14L3NpdGlvX2ludGVybmV0L2NmZC8zL2NmZHYzMy54c2QiIHhtbG5zOmNmZGk9Imh0dHA6Ly93d3cuc2F0LmdvYi5teC9jZmQvMyI+CiAgPGNmZGk6RW1pc29yIFJmYz0iRUtVOTAwMzE3M0M5IiBOb21icmU9IkVTQ1VFTEEgS0VNUEVSIFVSR0FURSIgUmVnaW1lbkZpc2NhbD0iNjAxIi8+CiAgPGNmZGk6UmVjZXB0b3IgUmZjPSJCQUFFODIxMDA5TUMzIiBOb21icmU9IkIyQiBURUNOT0xPR1kiIFVzb0NGREk9IkcwMyIvPgogIDxjZmRpOkNvbmNlcHRvcz4KICAgIDxjZmRpOkNvbmNlcHRvIENsYXZlUHJvZFNlcnY9IjAxMDEwMTAxIiBDYW50aWRhZD0iMSIgQ2xhdmVVbmlkYWQ9IkU0OCIgRGVzY3JpcGNpb249IlBBR08gRU4gRE9MQVJFUyIgVmFsb3JVbml0YXJpbz0iMTAwIiBJbXBvcnRlPSIxMDAiPgogICAgICA8Y2ZkaTpJbXB1ZXN0b3M+CiAgICAgICAgPGNmZGk6VHJhc2xhZG9zPgogICAgICAgICAgPGNmZGk6VHJhc2xhZG8gQmFzZT0iMTAwIiBJbXB1ZXN0bz0iMDAyIiBUaXBvRmFjdG9yPSJUYXNhIiBUYXNhT0N1b3RhPSIwLjE2MDAwMCIgSW1wb3J0ZT0iMTYuMDAiLz4KICAgICAgICA8L2NmZGk6VHJhc2xhZG9zPgogICAgICA8L2NmZGk6SW1wdWVzdG9zPgogICAgPC9jZmRpOkNvbmNlcHRvPgogIDwvY2ZkaTpDb25jZXB0b3M+CiAgPGNmZGk6SW1wdWVzdG9zIFRvdGFsSW1wdWVzdG9zVHJhc2xhZGFkb3M9IjE2LjAwIj4KICAgIDxjZmRpOlRyYXNsYWRvcz4KICAgICAgPGNmZGk6VHJhc2xhZG8gSW1wdWVzdG89IjAwMiIgVGlwb0ZhY3Rvcj0iVGFzYSIgVGFzYU9DdW90YT0iMC4xNjAwMDAiIEltcG9ydGU9IjE2LjAwIi8+CiAgICA8L2NmZGk6VHJhc2xhZG9zPgogIDwvY2ZkaTpJbXB1ZXN0b3M+CiAgPGNmZGk6Q29tcGxlbWVudG8+CiAgICA8dGZkOlRpbWJyZUZpc2NhbERpZ2l0YWwgRmVjaGFUaW1icmFkbz0iMjAyMi0xMS0yM1QxMjo1MDoxMyIgVmVyc2lvbj0iMS4xIiBVVUlEPSJBRDU2NkU2MS0xQUU4LTQ3RUYtQjYyQi0wNjdBM0REM0EzNUYiIFJmY1Byb3ZDZXJ0aWY9IkNSRTExMDMwMkw1QSIgU2VsbG9DRkQ9IlZkRlJ5U2ZUOUdIMm9EWU40YnJZZXpLcW90dVZoRXZRMGRMQVphV0QycTBrM2RqVDlnYlUxeExhaGZyNjM2Y1FzZC9rQjU5TEF4dW5wUDBnREg1cjRBMm1MRm5OcXFxZmpMNHZjalY1VmwyWEFBRGFKWUpvOFpDMVlxSXVaNjBaU3I5SmVtcEVvdGh0SzBXWDdDZWFjNXZ5L2JaaVgxUllvR1pUM3R1b2NNSVFwUDZ5R0dmMTZCMjdFN2J3RDFsdTJjZ2xQTlIzNHkxQ0xzZXVIczFVYmJtbEd4NGNwNVZUNEU2a3NGSU9QaWFNbWtBU2VUeXU1UkVOUG40S3FGZlM5SWdFTTdPNWNkTEVOQWZDYXdISzVLT2hjNjVEbWR6OVprK0VhNDl0RUlOSXBLSUExaW93TVJEQmFGZkNKTEEvdzBUdkV0Q1BabUN2ekhXbjBvc2hKdz09IiBOb0NlcnRpZmljYWRvU0FUPSIzMDAwMTAwMDAwMDQwMDAwMjQ5NSIgU2VsbG9TQVQ9Ild2UkpXcXh6TElSbFpOdWVURnNvSm9MZk5EakpMdG9Kd3pIUkR1U3A4bUIzWXJOUVZDcHhXUms0UW1nUWZrcEI0S3YzUjNyTXZndHRJWXdyaW9JT0FydVZZRGlvYnI3Vks1Z3MyeXhSMkhLekNxSXhicHNxNmpoMVVCUFlHSlVIRG5TTGRvSXloRkg2bE91WkRUMVhUeDl4RlB2bTdWUVd5WlcwMUtOenROT3d4dHU3dURsaVlQckFFVjhrVFpjSG1pZDlaWTNkRVVEeUpBMi9HWDhzL1JITVZYWWJkSjNFeTJjZC9BUlRrVlc5L25RSkhicXpWNVUxcDEyeUU0a3IvOUZjd29GalF1djFydmFBVHExbHlOTk1ZZWw5VHBDeENUYlB0d0RPaWZuUmkxbDBvclVmTHU5b0JlamZWQW8xQkszc0RjcnBxSk5XaTRmMjFxc0lydz09IiB4bWxuczp0ZmQ9Imh0dHA6Ly93d3cuc2F0LmdvYi5teC9UaW1icmVGaXNjYWxEaWdpdGFsIiB4c2k6c2NoZW1hTG9jYXRpb249Imh0dHA6Ly93d3cuc2F0LmdvYi5teC9UaW1icmVGaXNjYWxEaWdpdGFsIGh0dHA6Ly93d3cuc2F0LmdvYi5teC9zaXRpb19pbnRlcm5ldC9jZmQvVGltYnJlRmlzY2FsRGlnaXRhbC9UaW1icmVGaXNjYWxEaWdpdGFsdjExLnhzZCIgLz4KICA8L2NmZGk6Q29tcGxlbWVudG8+CjwvY2ZkaTpDb21wcm9iYW50ZT4=";



            ServiceExamen.MessageRequest input = new ServiceExamen.MessageRequest();
            input.xmlBase64 = base64XML;
            input.rfcEmisor = "EKU9003173C9";
            input.tipoEmisor = ServiceExamen.TipoEmisor.Provider;
            input.operacion = ServiceExamen.Operacion.EmisionRetencion;
            

            var resultFile = service.ProcessDocument(input);
            //  var result = new FileResult();
            return View();
        }



    }
}
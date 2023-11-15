using DL;
using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Addendas
    {

        public static ML.Result GetAllLQ()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.Cer_AddendasEntities1 context = new DL.Cer_AddendasEntities1())
                {
                    var resultQuery = (from addendas in context.Addendas

                                       select new
                                       {
                                           addendas.IdAddenda,
                                           addendas.NombreAddenda,
                                         //  addendas.XML,
                                           addendas.FechaModificacion,
                                           addendas.Usuario,
                                           addendas.Estado
                                       }).ToList();

                    result.Objects = new List<object>();

                    if (resultQuery != null && resultQuery.ToList().Count > 0)
                    {
                        foreach (var obj in resultQuery)
                        {
                            ML.Addendas addendasItem = new ML.Addendas();

                            addendasItem.IdAddenda = obj.IdAddenda;
                            addendasItem.NombreAddenda = obj.NombreAddenda;
                          //  addendasItem.XML = obj.XML;
                            addendasItem.FechaModificacion = (obj.FechaModificacion != null) ? obj.FechaModificacion : null;
                            addendasItem.Usuario = obj.Usuario;
                            addendasItem.Estado = (obj.Estado != null) ? obj.Estado : false;

                            result.Objects.Add(addendasItem);
                        }

                        result.Correct = true;

                    }

                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay registros";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;

            }
            return result;
        }


        public static ML.Result Add(ML.Addendas addendas)
        {
            Result result = new Result();
            try
            {
                using (DL.Cer_AddendasEntities1 context = new DL.Cer_AddendasEntities1())
                {

                    DL.Addenda addendasdl = new DL.Addenda();

                    addendasdl.IdAddenda = Guid.NewGuid();
                    addendasdl.NombreAddenda = addendas.NombreAddenda;
                    addendasdl.FechaModificacion = addendas.FechaModificacion;
                    addendasdl.XML = addendas.XML;
                    addendasdl.Usuario = addendas.Usuario;
                    addendasdl.Estado = addendas.Estado;
                    context.Addendas.Add(addendasdl);
                    context.SaveChanges();
                    result.Correct = true;


                }
            }

            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }
            return result;
        }


        public static ML.Result GetByIdLQ(Guid IdAddenda)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (Cer_AddendasEntities1 context = new Cer_AddendasEntities1())
                {
                    var resultQuery = (from addenda in context.Addendas
                                       where addenda.IdAddenda == IdAddenda
                                       select addenda).First();
                    result.Objects = new List<object>();

                    if (resultQuery != null)
                    {

                        ML.Addendas addendas = new ML.Addendas();

                        addendas.IdAddenda = resultQuery.IdAddenda;
                        addendas.NombreAddenda = resultQuery.NombreAddenda;
                        addendas.XML = resultQuery.XML;
                        addendas.FechaModificacion = resultQuery.FechaModificacion;
                        addendas.Usuario = resultQuery.Usuario;
                        addendas.Estado = resultQuery.Estado;

                        result.Object = addendas;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontro";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }


        public static ML.Result UpdateLQ(ML.Addendas addendas)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (Cer_AddendasEntities1 context = new Cer_AddendasEntities1())
                {
                    var resultQuery = (from addenda in context.Addendas
                                       where addenda.IdAddenda == addendas.IdAddenda
                                       select addenda).Single();

                    if (resultQuery != null)
                    {

                        resultQuery.IdAddenda = addendas.IdAddenda;
                        resultQuery.NombreAddenda = addendas.NombreAddenda;
                        resultQuery.XML = addendas.XML;
                        resultQuery.FechaModificacion = addendas.FechaModificacion;
                        resultQuery.Usuario = addendas.Usuario;
                        resultQuery.Estado = addendas.Estado;
                        resultQuery.Estado = addendas.Estado;


                        context.SaveChanges();
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontro el registro";
                    }

                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }




    }
}

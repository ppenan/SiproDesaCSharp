using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using SiproModelCore.Models;
using System.Data.Common;
using Utilities;

namespace SiproDAO.Dao
{
    public class SubproductoUsuarioDAO
    {
        //MODIFICAR 03/10/2018
        public static bool guardarSubproductoUsuario(SubproductoUsuario subproductoUsuario)
        {
            bool ret = false;
            /*Session session = CHibernateSession.getSessionFactory().openSession();
            try
            {
                session.beginTransaction();
                session.saveOrUpdate(subproductoUsuario);
                session.getTransaction().commit();
                ret = true;
            }
            catch (Exception e)
            {
                CLogger.write("1", "SubproductoUsuarioDAO.class", e);
		    }*/
		    
		    return ret;
	    }



        //MODIFICAR 03/10/2018      
        public static bool eliminarTotalProductoUsuario(SubproductoUsuario subproductoUsuario)
        {
            bool ret = false;
            /*Session session = CHibernateSession.getSessionFactory().openSession();
            try
            {
                session.beginTransaction();
                session.delete(subproductoUsuario);
                session.getTransaction().commit();
                ret = true;
            }
            catch (Exception e)
            {
                CLogger.write("2", "SubproductoUsuarioDAO.class", e);
		    }
		    finally
            {
			    session.close();
		    }*/
		    return ret;
	    }
    }
}

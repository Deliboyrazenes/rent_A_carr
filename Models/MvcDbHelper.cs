using EntityLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rent_A_carr.Models
{
    public sealed class MvcDbHelper
    {
        //referans ekle alt+enter
        private static volatile AbstractDapperRepository _repositoryInstance;
        private static object _syncRoot = new object();

        public MvcDbHelper()
        {
            
        }

        public static AbstractDapperRepository Repository
        {
            get
            {
                if (_repositoryInstance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_repositoryInstance == null)
                            _repositoryInstance = new DapperRepository("ConnectionName");
                    }

                }
                return _repositoryInstance;
            }
        }
    }
}
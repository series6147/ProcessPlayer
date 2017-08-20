using Oracle.ManagedDataAccess.Client;
using ProcessPlayer.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessPlayer.Content.Common
{
    public class ToRdOracle : ProcessContent
    {
        #region private variables

        private CommandType _commandType;
        private Dictionary<string, string> _parameters;
        private string _connectionString;
        private string _dataSource;
        private string _definition;
        private string _password;
        private string _userID;

        #endregion

        #region private methods

        private async Task<DataExchangeObject[]> perform()
        {
            var token = CancellationToken.Token;

            return Allowed()
                ? await Task<DataExchangeObject[]>.Factory.StartNew(() =>
                {
                    var connection = string.IsNullOrEmpty(ConnectionString)
                        ? new OracleConnection(string.Format("Data Source={0};User Id={1};Password={2};Pooling=true;", DataSource, UserID, Password))
                        : new OracleConnection(ConnectionString);
                    var res = new DataExchangeObject() { ID = ID };

                    if (token.IsCancellationRequested)
                        return null;

                    try
                    {
                        connection.Open();

                        using (var command = new OracleCommand(Definition, connection) { CommandTimeout = 0, CommandType = CommandType })
                        {
                            if (Parameters != null)
                                command.Parameters.AddRange(Parameters.Select(kvp => new OracleParameter(kvp.Key, Vars[kvp.Value])).ToArray());

                            if (token.IsCancellationRequested)
                                return null;

                            res.Data = new CanceledReader(command.ExecuteReader(CommandBehavior.CloseConnection), new CancellationToken[] { token });
                        }
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        connection.Dispose();

                        Root.Log.Error(string.Format("{0} - execution\r\n{1}", ID, ex));

                        Cancel();
                    }

                    return new DataExchangeObject[] { res };
                }, token)
                : null;
        }

        #endregion

        #region properties

        public CommandType CommandType
        {
            get { return _commandType; }
            set
            {
                if (_commandType != value)
                {
                    _commandType = value;

                    RaisePropertyChanged("CommandType");
                }
            }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                if (_connectionString != value)
                {
                    _connectionString = value;

                    RaisePropertyChanged("ConnectionString");
                }
            }
        }

        public string DataSource
        {
            get { return _dataSource; }
            set
            {
                if (_dataSource != value)
                {
                    _dataSource = value;

                    RaisePropertyChanged("DataSource");
                }
            }
        }

        public string Definition
        {
            get { return _definition; }
            set
            {
                if (_definition != value)
                {
                    _definition = value;

                    RaisePropertyChanged("Definition");
                }
            }
        }

        public Dictionary<string, string> Parameters
        {
            get { return _parameters; }
            set
            {
                _parameters = value;

                RaisePropertyChanged("Parameters");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;

                    RaisePropertyChanged("Password");
                }
            }
        }

        public string UserID
        {
            get { return _userID; }
            set
            {
                if (_userID != value)
                {
                    _userID = value;

                    RaisePropertyChanged("UserID");
                }
            }
        }

        #endregion

        #region constructors

        public ToRdOracle()
        {
            CommandType = CommandType.Text;
        }

        #endregion

        #region ProcessContent Members

        public override async Task Diagnostics()
        {
            await Task.Run(() =>
            {
                using (var connection = string.IsNullOrEmpty(ConnectionString)
                    ? new OracleConnection(string.Format("Data Source={0};User Id={1};Password={2};Pooling=true;", DataSource, UserID, Password))
                    : new OracleConnection(ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        Root.Log.Error(string.Format("{0} - diagnostic\r\n{1}", ID, ex));

                        throw;
                    }
                }
            });
        }

        public override async Task<DataExchangeObject[]> ExecuteAsync()
        {
            var token = CancellationToken.Token;

            if (token.IsCancellationRequested)
                return null;

            IsExecuted = true;

            try
            {
                foreach (var r in OutgoingLinks)
                    r.IncomingDataBuffer.Remove(ID);

                switch ((Method ?? string.Empty).ToLower())
                {
                    default:
                        base.RaiseExecuteStarted();

                        var res = await perform();

                        base.RaiseExecuteFinished();

                        if (res != null)
                            foreach (var r in OutgoingLinks)
                                r.IncomingDataBuffer[ID] = res;

                        return res;
                }
            }
            catch (Exception ex)
            {
                Root.Log.Error(string.Format("{0} - execution\r\n{1}", ID, ex));

                Cancel();
            }
            finally
            {
                IsExecuted = false;
            }

            return null;
        }

        #endregion
    }
}

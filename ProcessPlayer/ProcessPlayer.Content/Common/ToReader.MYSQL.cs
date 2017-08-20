using MySql.Data.MySqlClient;
using ProcessPlayer.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessPlayer.Content.Common
{
    public class ToRdMYSQL : ProcessContent
    {
        #region private variables

        private bool _integratedSecurity;
        private CommandType _commandType;
        private Dictionary<string, string> _parameters;
        private string _connectionString;
        private string _database;
        private string _definition;
        private string _password;
        private string _server;
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
                        ? new MySqlConnection(string.Format("Server={0};Initial Catalog={1};User ID={2};Password={3};Integrated Security={4};", Server, Database, UserID, Password, IntegratedSecurity))
                        : new MySqlConnection(ConnectionString);
                    var res = new DataExchangeObject() { ID = ID };

                    if (token.IsCancellationRequested)
                        return null;

                    try
                    {
                        connection.Open();

                        using (var command = new MySqlCommand(Definition, connection) { CommandTimeout = 0, CommandType = CommandType })
                        {
                            if (Parameters != null)
                                command.Parameters.AddRange(Parameters.Select(kvp => new MySqlParameter(kvp.Key, Vars[kvp.Value])).ToArray());

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

                    RaisePropertyChangedAsync("CommandType");
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

                    RaisePropertyChangedAsync("ConnectionString");
                }
            }
        }

        public string Database
        {
            get { return _database; }
            set
            {
                if (_database != value)
                {
                    _database = value;

                    RaisePropertyChangedAsync("Database");
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

                    RaisePropertyChangedAsync("Definition");
                }
            }
        }

        public bool IntegratedSecurity
        {
            get { return _integratedSecurity; }
            set
            {
                if (_integratedSecurity != value)
                {
                    _integratedSecurity = value;

                    RaisePropertyChangedAsync("IntegratedSecurity");
                }
            }
        }

        public Dictionary<string, string> Parameters
        {
            get { return _parameters; }
            set
            {
                _parameters = value;

                RaisePropertyChangedAsync("Parameters");
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

                    RaisePropertyChangedAsync("Password");
                }
            }
        }

        public string Server
        {
            get { return _server; }
            set
            {
                if (_server != value)
                {
                    _server = value;

                    RaisePropertyChangedAsync("Server");
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

                    RaisePropertyChangedAsync("UserID");
                }
            }
        }

        #endregion

        #region constructors

        public ToRdMYSQL()
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
                    ? new MySqlConnection(string.Format("Server={0};Initial Catalog={1};User ID={2};Password={3};Integrated Security={4};", Server, Database, UserID, Password, IntegratedSecurity))
                    : new MySqlConnection(ConnectionString))
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

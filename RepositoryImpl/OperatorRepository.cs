using BT_COMMONS.Database;
using BT_COMMONS;
using BT_COMMONS.DataRepositories;
using BT_COMMONS.Operators;
using BT_COMMONS.Operators.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Crypto.Generators;

namespace BT_BO.RepositoryImpl;

public class OperatorRepository : IOperatorRepository
{
    private readonly DatabaseAccess _database;

    public OperatorRepository(DatabaseAccess database)
    {
        _database = database;
    }

    public async Task<Operator?> GetOperator(int id)
    {
        var opers = await _database.LoadData<Operator, dynamic>("SELECT id, isactive, operatorid, operatorpassword, istemppassword, firstname, nickname, lastname, groupsid, dateofbirth, hiredate, terminationdate FROM `operators` WHERE `id`=?;", new { id });
        if (opers.Count == 0)
        {
            return null;
        }

        var controller = App.AppHost.Services.GetRequiredService<BOController>();
        var oper = opers[0].Parse(controller.OperatorGroups);

        return oper;
    }

    public async Task<List<OperatorGroup>> GetOperatorGroups()
    {
        List<OperatorGroup> operGroups = await _database.LoadData<OperatorGroup, dynamic>("SELECT * FROM `groups`;", new { });
        return operGroups;
    }

    public async Task<OperatorLoginResponse> OperatorLogin(OperatorLoginRequest request)
    {
        var opers = await _database.LoadData<Operator, dynamic>("SELECT id, isactive, operatorid, operatorpassword, groupsid FROM `operators` WHERE `operatorid`=?;", new { request.Id });
        if (opers.Count == 0)
        {
            return new OperatorLoginResponse
            {
                ID = null,
                Message = "Invalid operator id or password."
            };
        }

        var controller = App.AppHost.Services.GetRequiredService<BOController>();
        var oper = opers[0].Parse(controller.OperatorGroups);

        if (!oper.IsActive)
        {
            return new OperatorLoginResponse
            {
                ID = null,
                Message = "Operator is disabled."
            };
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, oper.OperatorPassword))
        {
            return new OperatorLoginResponse
            {
                ID = null,
                Message = "Invalid operator id or password."
            };
        }

        if (!oper.HasBoolPermission(OperatorBoolPermission.BO_Access))
        {
            return new OperatorLoginResponse
            {
                ID = null,
                Message = "Insufficient permission to use back office."
            };
        }

        return new OperatorLoginResponse
        {
            ID = oper.Id,
            Message = string.Empty
        };
    }
}

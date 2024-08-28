using BT_COMMONS.DataRepositories;
using BT_COMMONS.Operators;
using BT_COMMONS.Transactions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT_BO;

public class BOController
{
    private readonly IOperatorRepository _operatorRepository;

    public int StoreNumber { get; set; }

    public Operator? CurrentOperator { get; set; }
    public Dictionary<int, OperatorGroup>? OperatorGroups { get; set; }

    public BOController(IOperatorRepository operatorRepository)
    {
        _operatorRepository = operatorRepository;

        OperatorGroups = new Dictionary<int, OperatorGroup>();
    }

    public void HeaderError(string? error = null)
    {
        MainWindow mw = App.AppHost.Services.GetRequiredService<MainWindow>();
        mw.HeaderError(error);
    }

    public async Task<bool> CompleteLogin(int id)
    {
        var oper = await _operatorRepository.GetOperator(id);
        if (oper == null)
        {
            return false;
        }

        CurrentOperator = oper;
        MainWindow mw = App.AppHost.Services.GetRequiredService<MainWindow>();
        mw.BOParentHeader_Operator.Text = "Operator# " + oper.OperatorId;
        return true;
    }
}

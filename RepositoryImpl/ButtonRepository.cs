using BT_BO.Buttons.Menu;
using BT_COMMONS.Database;
using BT_COMMONS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using BT_BO.Buttons.Reports;

namespace BT_BO.RepositoryImpl;

public class ButtonRepository : IButtonRepository
{
    private readonly DatabaseAccess _database;

    public ButtonRepository(DatabaseAccess database)
    {
        _database = database;
    }

    public async Task<List<HomeButton>?> GetHomeButtons()
    {
        var tables = await _database.LoadData<string, dynamic>("SELECT buttons FROM `bo_buttons` WHERE `menu`=\"home\";", new { });
        if (tables.Count == 0)
        {
            return null;
        }

        Trace.WriteLine(tables[0]);
        var buttons = JsonConvert.DeserializeObject<List<HomeButton>>(tables[0]);
        return buttons;
    }

    public async Task<List<ReportSelectionButton>?> GetReportSelectionButtons()
    {
        var tables = await _database.LoadData<string, dynamic>("SELECT buttons FROM `bo_buttons` WHERE `menu`=\"report_selection\";", new { });
        if (tables.Count == 0)
        {
            return null;
        }

        Trace.WriteLine(tables[0]);
        var buttons = JsonConvert.DeserializeObject<List<ReportSelectionButton>>(tables[0]);
        return buttons;
    }
}

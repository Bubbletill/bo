using BT_BO.Buttons.Menu;
using BT_BO.Buttons.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT_BO.RepositoryImpl;

public interface IButtonRepository
{
    Task<List<HomeButton>?> GetHomeButtons();
    Task<List<ReportSelectionButton>?> GetReportSelectionButtons();
}

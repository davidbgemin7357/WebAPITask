using System;
using System.Collections.Generic;

namespace WebAPITask.Models;

public partial class Task
{
    public int n_id { get; set; }
    public int? n_employee_id { get; set; }
    public string s_name { get; set; } = null!;
    public string s_lastname { get; set; } = null!;
    public string s_title { get; set; } = null!;
    public string s_description { get; set; } = null!;
    public DateTime d_created { get; set; }
    public bool b_status { get; set; }
    public DateTime? d_updated { get; set; }

}


public class TaskDto
{
    public int n_id { get; set; }
    public int n_employee_id { get; set; }
    public int n_user_id { get; set; }
    public string s_title { get; set; }
    public string s_description { get; set; }
}




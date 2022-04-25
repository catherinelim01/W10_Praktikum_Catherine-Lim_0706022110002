using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace W8_Praktikum_Catherine_Lim_0706022110002
{
    public partial class HasilPertandingan : Form
    {
        public static string sqlConnection = "server=localhost;uid=root; pwd=;database=premier_league";
        public MySqlConnection sqlConnect = new MySqlConnection(sqlConnection);
        public MySqlCommand sqlCommand;
        public MySqlDataAdapter sqlAdapter;
        public string sqlQuery;

        public HasilPertandingan()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnect.Open();
            DataTable dtTeamHome = new DataTable();
            DataTable dtTeamAway = new DataTable();
          
            sqlQuery = "SELECT team_name as `NamaTim`, team_id as `ID Team` FROM team";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtTeamHome);

            sqlQuery = "SELECT team_name as `NamaTim`, team_id as `ID Team` FROM team";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtTeamAway);

            comboBox_Tim1.DataSource = dtTeamHome;
            comboBox_Tim2.DataSource = dtTeamAway;

            comboBox_Tim1.DisplayMember = "NamaTim";
            comboBox_Tim1.ValueMember = "ID Team";

            comboBox_Tim2.DisplayMember = "NamaTim";
            comboBox_Tim2.ValueMember = "ID Team";

        }

        private void comboBox_Tim1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
            DataTable dtPlayer = new DataTable(); 
            sqlQuery = "select m.manager_id, m.manager_name, t.team_id, t.captain_id, p.player_name, team_name, concat (t.home_stadium, ',', t.city) as home_stadium, t.capacity from manager m, team t, player p where m.manager_id = t.manager_id and t.captain_id = p.player_id and t.team_id = '" + comboBox_Tim1.SelectedValue +"'; ";

            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtPlayer);

            label_Captain1.Text = dtPlayer.Rows[0]["player_name"].ToString();
            label_Manager1.Text = dtPlayer.Rows[0]["manager_name"].ToString();

            label_Stadium.Text = dtPlayer.Rows[0]["home_stadium"].ToString();
            label_Capacity.Text = dtPlayer.Rows[0]["capacity"].ToString();

            }
            catch (Exception)
            {

               
            }
            
        }

        private void comboBox_Tim2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
            DataTable dtPlayer = new DataTable();
            sqlQuery = "select m.manager_id, m.manager_name, t.team_id, t.captain_id, p.player_name, team_name from manager m, team t, player p where m.manager_id = t.manager_id and t.captain_id = p.player_id and t.team_id = '" + comboBox_Tim2.SelectedValue.ToString() + "'; ";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtPlayer);

            label_Captain2.Text = dtPlayer.Rows[0]["player_name"].ToString();
            label_Manager2.Text = dtPlayer.Rows[0]["manager_name"].ToString();
            }
            catch (Exception)
            {


            }

        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtMatch = new DataTable();
                sqlQuery = "select date_format(match_date, '%e %M %Y') as 'Tanggal', concat(goal_home, ' - ', goal_away) as 'Skor' from `match` where team_home = '" + comboBox_Tim1.SelectedValue + "' and team_away = '" + comboBox_Tim2.SelectedValue + "';";

                sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
                sqlAdapter = new MySqlDataAdapter(sqlCommand);
                sqlAdapter.Fill(dtMatch);

                label_Tanggal.Text = dtMatch.Rows[0]["Tanggal"].ToString();
                label_Skor.Text = dtMatch.Rows[0]["Skor"].ToString();

                DataTable dtPertandingan = new DataTable();
                sqlQuery = "select d.minute as 'Minute', if (p1.team_id = '" + comboBox_Tim1.SelectedValue.ToString() + "', p1.player_name, '') as 'Player Name 1', if (p1.team_id = '" + comboBox_Tim1.SelectedValue.ToString() + "', if (d.type = 'CY', 'YELLOW CARD', if (d.type = 'CR', 'RED CARD', if (d.type = 'GO', 'GOAL', if (d.type = 'GP', 'GOAL PENALTY', if (d.type = 'GW', 'OWN GOAL', if (d.type = 'PM', 'PENALTY MISS', ' ')))))), '') as 'Type 1', if (p2.team_id = '" + comboBox_Tim2.SelectedValue.ToString() + "', p2.player_name, '') as 'Player Name 2', if (p2.team_id = '" + comboBox_Tim2.SelectedValue.ToString() + "', if (d.type = 'CY', 'YELLOW CARD', if (d.type = 'CR', 'RED CARD', if (d.type = 'GO', 'GOAL', if (d.type = 'GP', 'GOAL PENALTY', if (d.type = 'GW', 'OWN GOAL', if (d.type = 'PM', 'PENALTY MISS', ' ')))))), '') as 'Type 2' from dmatch d, player p1, player p2 where d.player_id = p1.player_id and d.player_id = p2.player_id and d.match_id = (select match_id from `match` where team_home = '" + comboBox_Tim1.SelectedValue.ToString() + "' and team_away = '" + comboBox_Tim2.SelectedValue.ToString() + "') and d.type != 'GW' group by 1 union select d.minute as 'Minute', if (p1.team_id = '" + comboBox_Tim2.SelectedValue.ToString() + "', p1.player_name, '') as 'Player Name 1', if (p1.team_id = '" + comboBox_Tim2.SelectedValue.ToString() + "', if (d.type = 'CY', 'YELLOW CARD', if (d.type = 'CR', 'RED CARD', if (d.type = 'GO', 'GOAL', if (d.type = 'GP', 'GOAL PENALTY', if (d.type = 'GW', 'OWN GOAL', if (d.type = 'PM', 'PENALTY MISS', ' ')))))), '') as 'Type 1', if (p2.team_id = '" + comboBox_Tim1.SelectedValue.ToString() + "', p2.player_name, '') as 'Player Name 2', if (p2.team_id = '" + comboBox_Tim1.SelectedValue.ToString() + "', if (d.type = 'CY', 'YELLOW CARD', if (d.type = 'CR', 'RED CARD', if (d.type = 'GO', 'GOAL', if (d.type = 'GP', 'GOAL PENALTY', if (d.type = 'GW', 'OWN GOAL', if (d.type = 'PM', 'PENALTY MISS', ' ')))))), '') as 'Type 2' from dmatch d, player p1, player p2 where d.player_id = p1.player_id and d.player_id = p2.player_id and d.match_id = (select match_id from `match` where team_home = '" + comboBox_Tim1.SelectedValue.ToString() + "' and team_away = '" + comboBox_Tim2.SelectedValue.ToString() + "') and d.type = 'GW' group by 1 order by 1; ";

                sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
                sqlAdapter = new MySqlDataAdapter(sqlCommand);
                sqlAdapter.Fill(dtPertandingan);

                dataGridViewPertandingan.DataSource = dtPertandingan;
            }
            catch (Exception)
            {


            }
        }
    }
}

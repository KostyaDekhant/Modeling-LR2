using Microsoft.VisualBasic.Logging;
using System;
using System.Data;
using System.Drawing;
using System.Security.Policy;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Modeling_LR2
{
    public partial class Form1 : Form
    {

        private int n, m;
        private int[][] data;
        private int[] order;

        private int tpr;
        private int tozh;

        int[][] fin_Time;
        int[][] d_time;

        Detail[] details;
        private Color[] colors = { Color.Red, Color.Green, Color.Blue, Color.Orange, Color.Brown, Color.Purple, Color.LightBlue, Color.Black};
        private List<PictureBox> picInp = new List<PictureBox>();
        private List<PictureBox> picOut = new List<PictureBox>();
        private List<Label> lbs = new List<Label>();

        private int pic_size = 10;
        private int height = 30;

        private string path = @"C:\Users\dekha\source\repos\Modeling LR2\Modeling LR2\";
        public Form1(){
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e){
            GetData("inp.txt");
            //Инициализация всех таблиц (устанавливаются параметры отображения)
            InitGrid(grid_inp, n, m);
            InitGrid(grid_out, n, m);
            InitGrid(grid_psl, n, m);
            InitGrid(grid_orders, n, m);

            //Для всех таблиц указываем количество столбцов и иные индивидуальные параметры
            InitGridInpOut(grid_inp);
            InitGridInpOut(grid_out);
            InitGridPsl(grid_psl);
            InitGridOrders(grid_orders);

            //Вычисление параметров P1, P2, alpha
            details = new Detail[n];
            CalcPs(data, grid_psl);

            //Устанавливаем координаты таблиц
            int x = grid_inp.Location.X + grid_inp.Size.Width + 5;
            int y = grid_inp.Location.Y;

            grid_psl.Location = new Point(x, y);

            x = grid_psl.Location.X + grid_psl.Size.Width + 5;
            y = grid_psl.Location.Y;

            grid_orders.Location = new Point(x, y);

            grid_inp.ReadOnly = true;
            grid_inp.ClearSelection();

            DrawDesign(inp_panel, picInp, lbs);

            
            int[][] d_time = new int[n][];
            order = new int[n];
            for (int i = 0; i < n; i++)
            {
                order[i] = i;
                d_time[i] = new int[m];
            }
            //Заполнение начальной таблицы значениями из файла
            FillGrid(grid_inp);

            //вычисляем время окончания при помощи матричного метода
            fin_Time = fin_time_calc();
            min_time = getFinishTime();

            //Отображение времени простоя, ожидания и окончания для начальной таблицы
            PrintStartTime();
            tpr = idleTime_calc();
            tozh = waitTime_calc();
            PrintTpr();
            PrintTozh();

            //Вычисление простоя
            d_time = d_time_calc();
            for (int i = 0; i < n; i++){
                DrawLine(i, data, d_time, true, inp_panel, picInp);
            }
            
            //Применение правил Петрова
            ApplyRules();

            //Отображение информации СПЗ и полного перебора
            DisplayShuffleInfo();
            DisplayFullEnumInfo();

            grid_orders.Columns[min_id].DefaultCellStyle.BackColor = Color.Gray;
        }

        //Таблица параметров
        private void InitGridPsl(DataGridView Grid)
        {
            Grid.Columns.Add("Pi1", "Pi1");
            Grid.Columns.Add("Pi2", "Pi2");
            Grid.Columns.Add("αi", "αi");

            for (int i = 0; i < n; i++)
            {
                Grid.Rows.Add();
                Grid.Rows[i].ReadOnly = true;
            }

            for (int i = 0; i < 3; i++) Grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            int height = Grid.Rows.Count * Grid.Rows[0].Height + Grid.ColumnHeadersHeight + 2;
            Grid.Height = height;
        }
        //Таблица с порядком деталей, полученным при помощи правил Петрова
        private void InitGridOrders(DataGridView Grid)
        {
            Grid.Columns.Add("1", "1");
            Grid.Columns.Add("2", "2");
            Grid.Columns.Add("3", "3");
            Grid.Columns.Add("4", "4");
            Grid.Columns.Add("spz", "spz");
            Grid.Columns.Add("fe", "fe");

            for (int i = 0; i < n; i++)
            {
                Grid.Rows.Add();
                Grid.Rows[i].ReadOnly = true;
            }

            for (int i = 0; i < 4; i++) Grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            int height = Grid.Rows.Count * Grid.Rows[0].Height + Grid.ColumnHeadersHeight + 2;
            Grid.Height = height;
        }
        //Таблица входных и выходных данных
        private void InitGridInpOut(DataGridView Grid){
            Grid.Columns.Add("№", "№");
            for (int i = 0; i < m; i++){
                Grid.Columns.Add((i + 1).ToString(), (i + 1).ToString());
            }
            for (int i = 0; i < n; i++){
                Grid.Rows.Add();
                Grid.Rows[i].Cells[0].Value = (i + 1).ToString();
                Grid.Rows[i].ReadOnly = true;
            }
            for (int i = 0; i < m; i++) Grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            
            int height = Grid.Rows.Count * Grid.Rows[0].Height + Grid.ColumnHeadersHeight + 2;
            Grid.Height = height;
        }
        //Общая инициализация для всех таблиц
        public void InitGrid(DataGridView Grid, int n, int m){
            Grid.Rows.Clear();
            Grid.Columns.Clear();

            Grid.ScrollBars = ScrollBars.None;
            Grid.RowHeadersVisible = false;
            Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Grid.AllowUserToAddRows = false;
            Grid.AllowUserToDeleteRows = false;
            Grid.MultiSelect = false;
            Grid.AllowUserToResizeRows = false;
            Grid.AllowUserToResizeColumns = false;
            Grid.AllowUserToOrderColumns = false;

            Grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 28, 28, 28);
            Grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(255, 234, 234, 234);



            Grid.DefaultCellStyle.BackColor = Color.FromArgb(255, 28, 28, 28);
            Grid.RowsDefaultCellStyle.ForeColor = Color.FromArgb(255, 234, 234, 234);
            Grid.BackgroundColor = Color.FromArgb(255, 28, 28, 28);

            Grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 28, 28, 28);

            Grid.EnableHeadersVisualStyles = false;
           
        }
        //Получение данных из файла
        private void GetData(String filename){
            string[] nums = File.ReadAllLines(path+filename);
            n = nums.Length;
            string[] m_count = nums[0].Split(' ');
            m = m_count.Length-1;

            data = new int[n][];
            for (int i = 0; i < n; i++){
                data[i] = new int[m];
            }
            for (int i = 0; i < n; i++){
                string[] nums_line = nums[i].Split(' ');
                for (int j = 0; j < m; j++){
                    data[i][j] = Convert.ToInt32(nums_line[j+1]);
                }
            }
        }
        //Заполнение таблицы данными из файла
        public void FillGrid(DataGridView Grid){
            Grid.Rows.Clear();
            Grid.RowCount = n;
            for (int i = 0; i < n; i++){
                Grid.Rows[i].Cells[0].Value = (order[i] + 1).ToString();
                for (int j = 0; j < m; j++){
                    Grid.Rows[i].Cells[j+1].Value = data[order[i]][j];
                }
            }
        }
        //Отрисовка квадратика для графика Ганта
        public void DrawSquare(Color color, Point loc, Panel pn, List<PictureBox> picList){
            PictureBox pb = new PictureBox();
            pn.Controls.Add(pb);

            pb.BackColor = color;
            pb.Location = loc;
            pb.Size = new Size(pic_size-1, pic_size-1);

            picList.Add(pb);
        }
        //Метод очистки всех квадратиков (не использовался)
        public void ClearPictureBoxes(List<PictureBox> picList, Panel pn){
            foreach (PictureBox pb in picList){
                pn.Controls.Remove(pb);
                pb.Dispose();
            }
            picList.Clear();
        }
        //Метод очистки всех значений над деталями (не использовался)
        public void ClearLbs(List<Label> lbs, Panel pn){
            foreach (Label lb in lbs){
                pn.Controls.Remove(lb);
                lb.Dispose();
            }
            lbs.Clear();
        }

        int x_off = 30, y_off = 40;
        //Отрисовка одного станка
        private void DrawLine(int row, int[][] Data, int[][] d_time, bool first, Panel pn, List<PictureBox> pic){
            int t_sum = 0;
            for (int i = 0; i < m; i++){
                if (first && d_time[row][i] != 0){
                    //Число над квадратами простоя
                    int shift = d_time[row][i] <= 2 ? t_sum : d_time[row][i] * pic_size / 2 + t_sum;
                    shift -= 3;
                    string text = d_time[row][i].ToString();
                    Point loc = new Point(x_off + i * pic_size + shift, y_off + row * height - 20);
                    Size size = d_time[row][i] < 10 ? new Size(12, 20) : new Size(25, 20);
                    DrawText(text, loc, size, pn, lbs, 100, 100, 100);

                    //Отрисовка квадратов простоя
                    for (int j = 0; j < d_time[row][i]; j++){
                        Point point = new Point(x_off + i* pic_size + t_sum, y_off + row* height);
                        DrawSquare(Color.Yellow, point, pn, pic);
                        t_sum += pic_size;
                    }
                }
                if (Data[order[i]][row] > 0){
                    //Число над квадратами станка
                    int shift = Data[order[i]][row] <= 2 ? t_sum : Data[order[i]][row] * pic_size/2 + t_sum;
                    shift -= 3;
                    string text = Data[order[i]][row].ToString();
                    Point loc = new Point(x_off + i * pic_size + shift, y_off + row * height - 20);
                    Size size = Data[order[i]][row] < 10 ? new Size(12, 20) : new Size(25, 20);
                    DrawText(text, loc, size, pn, lbs);
                }
                //Отрисовка квадратов деталей 
                for (int j = 0; j < Data[order[i]][row]; j++){
                    Point point = new Point(x_off + i * pic_size + t_sum, y_off + row * height);
                    DrawSquare(colors[order[i]], point, pn, pic);
                    t_sum += pic_size;
                }
                t_sum -= pic_size;
            }
            int sum = 0;
            for (int i = 0; i < m; i++){
                if(first) sum += d_time[row][i];
                sum += Data[order[i]][row];
            }
            if (sum > 0){
                //Общее время на станке
                string text = sum.ToString();
                Point loc = new Point(x_off + sum * pic_size + 10, y_off + row * height - 8);
                Size size = new Size(35, 20);
                DrawText(text, loc, size, pn, lbs);
            }
        }
        //Отрисовка остаточных элементов для отображения станков
        private void DrawDesign(Panel pn, List<PictureBox> picList, List<Label> lbs){
            PictureBox pb = new PictureBox();

            pb.BackColor = Color.Black;
            pb.Location = new Point(x_off-2, y_off-y_off/3);
            pb.Size = new Size(2, height * n);
            picList.Add(pb);
            pn.Controls.Add(pb);

            Label lb = new Label();
            Point loc = new Point(x_off - 25, y_off - 40);
            Size size = new Size(70, 20);
            DrawText("станки", loc, size, pn, lbs);

            //Отрисовка текста с номером станка 
            for (int i = 0; i < n; i++){
                string text = (i + 1).ToString();
                loc = new Point(x_off - 25, y_off - 7 + height * i);
                size = new Size(15, 20);
                DrawText(text, loc, size, pn, lbs);
            }
        }
        //Отрисовка текста
        private void DrawText(string text, Point loc, Size size, Panel pn, List<Label> lbs, int R = 0, int G = 0, int B = 0){
            Label lb_r = new Label();
            lb_r.Text = text;
            lb_r.Location = loc;
            lb_r.Size = size;
            lb_r.ForeColor = Color.FromArgb(255, R, G, B);
            pn.Controls.Add(lb_r);
            lbs.Add(lb_r);
        }
        //Матричный метод
        private int[][] fin_time_calc()
        {
            int[][] d_time = new int[n][];
            for (int i = 0; i < n; i++) d_time[i] = new int[m];
            for (int i = 0; i < n; i++){
                for (int j = 0; j < m; j++){
                    int detailID = order[i];
                    int procTime = data[detailID][j];
                    int temp = 0;
                    if (i == 0 && j > 0) temp = d_time[i][j - 1];
                    else if (i > 0 && j == 0) temp = d_time[i - 1][j];
                    else if (i > 0 && j > 0) temp = Math.Max(d_time[i][j - 1], d_time[i - 1][j]);
                    d_time[i][j] = procTime + temp;
                }
            }
            return d_time;
        }
        //Вычисление простоя 
        private int[][] d_time_calc()
        {
            int[][] d_time = new int[n][];
            for (int i = 0; i < n; i++) d_time[i] = new int[m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    int detailID = order[j];
                    int lastDetail = 0;
                    if (j > 0) lastDetail = fin_Time[j - 1][i];
                    d_time[i][j] = fin_Time[j][i] - lastDetail - data[detailID][i];
                }
            }
            return d_time;
        }
        //Подсчёт времени простоя
        int idleTime_calc()
        {
            int resTime = 0;
            for (int j = 0; j < n; j++)
            {
                int j_Time = 0;
                for (int i = 0; i < m; i++)
                {
                    j_Time += data[i][j];
                }
                int fin_Time_Id = n - 1;

                while (data[order[fin_Time_Id]][j] == 0) fin_Time_Id--;

                int machineTime = fin_Time[fin_Time_Id][j] - j_Time;
                resTime += machineTime;
            }

            return resTime;
        }
        //Вычисление времени ожидания
        int waitTime_calc()
        {
            int resTime = 0;
            for (int i = 0; i < n; i++)
            {
                int i_Time = 0;
                for (int j = 0; j < m; j++)
                {
                    i_Time += data[i][j];
                }
                int fin_Time_Id = n - 1;
                while (data[i][fin_Time_Id] == 0) fin_Time_Id--;
                int detailTime = fin_Time[i][fin_Time_Id] - i_Time;
                resTime += detailTime;
            }
            return resTime;
        }


        //Метод для применения Правила 1 Петрова
        private int[] Rule1(Detail[] details)
        {
            var D1_0 = details.Where(d => d.Lambda >= 0).OrderBy(d => d.Pi1).ToList();
            var D2 = details.Where(d => d.Lambda < 0).OrderByDescending(d => d.Pi2).ToList();

            // Объединяем результаты
            var orderedDetails = D1_0.Concat(D2).ToArray();
            return orderedDetails.Select(d => d.Index).ToArray();
        }

        //Метод для применения Правила 2 Петрова
        private int[] Rule2(Detail[] details)
        {
            var orderedDetails = details.OrderByDescending(d => d.Lambda).ToArray();
            return orderedDetails.Select(d => d.Index).ToArray();
        }

        //Метод для применения Правила 3 Петрова
        private int[] Rule3(Detail[] details)
        {
            var D1 = details.Where(d => d.Lambda > 0).OrderBy(d => d.Pi1).ToList();
            var D0 = details.Where(d => d.Lambda == 0).OrderBy(d => d.Pi1).ToList();
            var D2 = details.Where(d => d.Lambda < 0).OrderByDescending(d => d.Pi2).ToList();

            // Объединяем результаты
            var orderedDetails = D1.Concat(D0).Concat(D2).ToArray();
            return orderedDetails.Select(d => d.Index).ToArray();
        }

        //Метод для применения Правила 4 Петрова
        private int[] Rule4(Detail[] details)
        {
            var D1 = details.Where(d => d.Lambda > 0).ToList();
            var D0 = details.Where(d => d.Lambda == 0).ToList();
            var D2 = details.Where(d => d.Lambda < 0).ToList();

            List<int> orderedIndices = new List<int>();

            // Упорядочение D1
            while (D1.Count > 0)
            {
                if (D1.Count >= 2)
                {
                    // Выбираем первую деталь с максимальным Pi2
                    var firstDetail = D1.OrderByDescending(d => d.Pi2).First();
                    D1.Remove(firstDetail);

                    // Выбираем вторую деталь с минимальным Pi1
                    var secondDetail = D1.OrderBy(d => d.Pi1).First();
                    D1.Remove(secondDetail);

                    // Добавляем пару в список
                    orderedIndices.Add(firstDetail.Index);
                    orderedIndices.Add(secondDetail.Index);
                }
                else
                {
                    // Если осталась одна деталь, запоминаем ее
                    var remainingDetail = D1.First();
                    D1.Remove(remainingDetail);
                    orderedIndices.Add(remainingDetail.Index);

                    // Если D0 не пусто, добавляем деталь из D0
                    if (D0.Count > 0)
                    {
                        var detailFromD0 = D0.OrderBy(d => d.Pi1).First();
                        D0.Remove(detailFromD0);
                        orderedIndices.Add(detailFromD0.Index);
                    }
                    break;
                }
            }

            // Упорядочение D0
            while (D0.Count > 0)
            {
                if (D0.Count >= 2)
                {
                    var firstDetail = D0.OrderByDescending(d => d.Pi2).First();
                    D0.Remove(firstDetail);
                    var secondDetail = D0.OrderBy(d => d.Pi1).First();
                    D0.Remove(secondDetail);
                    orderedIndices.Add(firstDetail.Index);
                    orderedIndices.Add(secondDetail.Index);
                }
                else
                {
                    var remainingDetail = D0.First();
                    D0.Remove(remainingDetail);
                    orderedIndices.Add(remainingDetail.Index);

                    if (D2.Count > 0)
                    {
                        var detailFromD2 = D2.OrderBy(d => d.Pi1).First();
                        D2.Remove(detailFromD2);
                        orderedIndices.Add(detailFromD2.Index);
                    }
                    break;
                }
            }
            int remId = 0;
            // Упорядочение D2
            while (D2.Count > 0)
            {
                if (D2.Count >= 2)
                {
                    var firstDetail = D2.OrderByDescending(d => d.Pi2).First();
                    D2.Remove(firstDetail);
                    var secondDetail = D2.OrderBy(d => d.Pi1).First();
                    D2.Remove(secondDetail);
                    orderedIndices.Add(firstDetail.Index);
                    orderedIndices.Add(secondDetail.Index);
                }
                else
                {
                    remId = D2.First().Index;
                    break;
                }
            }

            // Вставка непарной детали, если количество всех деталей нечетное
            if (n % 2 != 0)
            {
                var lastDetailIndex = remId;//orderedIndices.Last();
                var lastDetail = details[lastDetailIndex];

                // Найти подходящие пары для вставки
                for (int i = 0; i < orderedIndices.Count-3; i ++)
                {
                    var firstPairDetail = details[orderedIndices[i]];
                    var secondPairDetail = details[orderedIndices[i + 1]];


                    if (Math.Max(firstPairDetail.Lambda, secondPairDetail.Lambda) >= lastDetail.Lambda &&
                                     lastDetail.Lambda >= Math.Min(details[orderedIndices[i + 2]].Lambda, details[orderedIndices[i + 3]].Lambda))
                    {
                        orderedIndices.Insert(i + 1, lastDetailIndex);
                        break;
                    }
                    else if (i == orderedIndices.Count - 4)
                    {
                        orderedIndices.Add(lastDetailIndex);
                        break;
                    }
                }
            }

            return orderedIndices.ToArray();
        }
        //Перестановка двух элементов массива
        void swap(int[] nums, int i, int j)
        {
            int s = nums[i];
            nums[i] = nums[j];
            nums[j] = s;
        }
        //Следующая последовательность
        bool nextSet(int[] nums)
        {
            int j = n - 2;
            while (j != -1 && nums[j] >= nums[j + 1]) j--;
            if (j == -1)
                return false; //больше перестановок нет 
            int k = n - 1;
            while (nums[j] >= nums[k]) k--;
            swap(nums, j, k);
            int l = j + 1, r = n - 1; //сортируем оставшуюся часть последовательности 
            while (l < r)
                swap(nums, l++, r--);
            return true;
        }

        private int min_time;
        private int min_id = 0;

        //Метод для вызова всех правил Петрова
        private void ApplyRules()
        {
            //Применение правил Петрова
            int[][] orders = new int[4][];
            orders[0] = Rule1(details);
            orders[1] = Rule2(details);
            orders[2] = Rule3(details);
            orders[3] = Rule4(details);


            //Мин. из 4-х правил
            order = orders[0];
            fin_Time = fin_time_calc();
            min_time = getFinishTime(); 
            for (int i = 0; i < 4; i++)
            {
                DisplayOrder(orders[i], i);
                order = orders[i];
                fin_Time = fin_time_calc();
                if (min_time > getFinishTime())
                {
                    min_time = getFinishTime();
                    min_id = i;
                }
            }

            //Отрисовка оптимального варианта
            order = orders[min_id];
            DrawDesign(out_panel, picOut, lbs);

            fin_Time = fin_time_calc();
            min_time = getFinishTime();
            d_time = d_time_calc();

            for (int i = 0; i < n; i++)
            {
                DrawLine(i, data, d_time, true, out_panel, picOut);
            }
            //Заполнение таблицы с оптимальным порядком для всех станков
            FillGrid(grid_out);

            //Отображение времени простоя, ожидания и окончания для правила Петрова
            PrintStartTime();
            tpr = idleTime_calc();
            tozh = waitTime_calc();
            PrintTpr();
            PrintTozh();
        }
        //Отображения СПЗ
        private void DisplayShuffleInfo()
        {
            //рандом
            order = Shuffle(); 
            fin_Time = fin_time_calc();
            min_time = getFinishTime();
            PrintStartTime();
            tpr = idleTime_calc();
            tozh = waitTime_calc();
            PrintTpr();
            PrintTozh();
            for (int j = 0; j < n; j++)
                grid_orders.Rows[j].Cells[4].Value = order[j] + 1;
        }
        //Отображение полного перебора
        private void DisplayFullEnumInfo()
        {
            //полный перебор
            int[] f_order = new int[n];
            for (int i = 0; i < n; i++)
            {
                order[i] = i;
                f_order[i] = i;
            };

            while (nextSet(order))
            {
                fin_Time = fin_time_calc();
                if (getFinishTime() < min_time)
                {
                    min_time = getFinishTime();
                    for (int i = 0; i < n; i++) f_order[i] = order[i];
                }
            }
            for (int i = 0; i < n; i++)
            {
                order[i] = f_order[i];
            }
            PrintStartTime();
            fin_Time = fin_time_calc();
            tpr = idleTime_calc();
            tozh = waitTime_calc();
            PrintTpr();
            PrintTozh();
            for (int j = 0; j < n; j++)
                grid_orders.Rows[j].Cells[5].Value = order[j] + 1;
        }

        //Перетасовка массива с использованием алгоритма Фишера-Йетса
        private int[] Shuffle()
        {
            int[] ord = new int[n];
            for (int i = 0; i < n; i++) ord[i] = i;
            Random rand = new Random();
            for (int i = ord.Length - 1; i > 0; i--)
            {
                //Генерируем случайный индекс
                int j = rand.Next(0, i + 1);
                //Меняем местами элементы
                swap(ord, i, j);
            }
            return ord;
        }
        //Получение время обработки деталей
        private int getFinishTime()
        {
            return fin_Time[n-1][m-1];
        }
        //Отображение времени обработки
        private void PrintStartTime()
        {
            start_time_lb.Text += "FTime " + min_time.ToString() + "\n";
        }
        //Отображение времени простоя
        private void PrintTpr()
        {
            tpr_lb.Text += "Tpr " + tpr.ToString() + "\n";
        }
        //Отображение времени ожидания
        private void PrintTozh()
        {
            tozh_lb.Text += "Tozh " + tozh.ToString() + "\n";
        }
        //Подсчёт параметров
        private void CalcPs(int[][] data, DataGridView grid)
        {
            int m_half = m % 2 == 0 ? m / 2 : (m + 1) / 2;
            for (int i = 0; i < n; i++)
            {
                int Pi1 = 0, Pi2 = 0;
                for (int j = 0; j < m_half; j++)
                {
                    Pi1 += data[i][j];
                }
                for (int j = m_half - 1; j < m; j++)
                {
                    Pi2 += data[i][j];
                }
                int alpha = Pi2 - Pi1;

                details[i] = new Detail(i, Pi1, Pi2);

                grid.Rows[i].Cells[0].Value = Pi1.ToString();
                grid.Rows[i].Cells[1].Value = Pi2.ToString();
                grid.Rows[i].Cells[2].Value = alpha.ToString();
            }
        }
        //Отображение порядков деталей
        private void DisplayOrder(int[] order, int rule)
        {
            for (int i = 0; i < order.Length; i++)
            {
                grid_orders.Rows[i].Cells[rule].Value = order[i] + 1; 
            }
        }
    }
}

using System;
class Program
{
    static void Main(string[] args)
    {
       int n = Convert.ToInt32(Console.ReadLine());
       int[] arr = new int[n];
       for(int i = 0; i < n; i++)
       {
           arr[i] = Convert.ToInt32(Console.ReadLine());
       }
       // Dùng long để tránh tràn số khi cộng n phần tử kiểu int (tối đa 100,000 phần tử)
        long sum = 0; 
        // Khởi tạo min, max từ phần tử đầu tiên thay vì 0 để tránh sai lệch khi toàn bộ mảng là số âm
        int min = arr[0];
        int max = arr[0];      
        // Ép kiểu (double) trước phép chia để giữ lại phần thập phân của trung bình cộng
        double average = (double)sum / n;
       // Khoi tao cac bien dem so phan tu am, duong, bang 0
        int CountNegative = 0;
        int CountPositive = 0;
        int CountZero = 0;
        // Vòng lặp duyệt qua đúng n phần tử một lần duy nhất để tối ưu thời gian tính toán
        for (int i = 0; i < n; i++)
        {
            int current = arr[i];

            // Cộng dồn phần tử hiện tại vào tổng
            sum += current;

            // Cập nhật giá trị nhỏ nhất nếu tìm thấy phần tử nhỏ hơn min hiện tại
            if (current < min)
            {
                min = current;
            }

            // Cập nhật giá trị lớn nhất nếu tìm thấy phần tử lớn hơn max hiện tại
            if (current > max)
            {
                max = current;
            }

            // Phân loại số âm, số dương và số 0
            if (current < 0)
            {
                CountNegative++;
            }
            else if (current > 0)
            {
                CountPositive++;
            }
            else
            {
                CountZero++;
            }
        }
        // Con trỏ left bắt đầu từ chỉ số đầu tiên của mảng
        int left = 0;

        // Con trỏ right bắt đầu từ chỉ số cuối cùng của mảng
        int right = n - 1;

        // Vòng lặp chạy khi left chưa gặp hoặc vượt qua right
        while (left < right)
        {
            // Lưu tạm giá trị bên trái vào biến temp để không bị ghi đè dữ liệu khi hoán đổi
            int temp = arr[left];

            // Gán giá trị từ vị trí right sang vị trí left
            arr[left] = arr[right];

            // Gán giá trị lưu tạm ở temp vào vị trí right
            arr[right] = temp;

            // Dịch con trỏ trái sang phải 1 bước để xét phần tử tiếp theo
            left++;

            // Dịch con trỏ phải sang trái 1 bước để xét phần tử tiếp theo
            right--;
        }

        // In mảng sau khi đã đảo ngược
        Console.Write("Mang sau khi dao: ");
        for (int i = 0; i < n; i++)
        {
            Console.Write(arr[i] + (i == n - 1 ? "" : " "));
        }
        Console.WriteLine("Tong: {0}", sum);
        Console.WriteLine("Trung binh cong: {0:F2}", average);
        Console.WriteLine("Gia tri nho nhat: {0}", min);
        Console.WriteLine("Gia tri lon nhat: {0}", max);
        Console.WriteLine("So phan tu am: {0}", CountNegative);
        Console.WriteLine("So phan tu duong: {0}", CountPositive);
        Console.WriteLine("So phan tu bang 0: {0}", CountZero);
        Console.WriteLine();
    }
}

using System;
using System.Text;

namespace Bai4
{
    public class DynamicArray<T>
    {
        // Mảng thật chứa dữ liệu; độ dài của nó chính là Capacity (số ô đã cấp phát).
        private T[] items;

        // Số phần tử hợp lệ đang có; luôn <= Capacity. Chỉ lớp này được sửa (private set).
        public int Count { get; private set; }

        // Dung lượng = số ô của mảng nội bộ, tự đồng bộ với items nên không cần biến riêng.
        public int Capacity => items.Length;

        // Số phần tử đã sao chép trong LẦN MỞ RỘNG gần nhất (phục vụ quan sát/khấu hao).
        public int LastCopiedCount { get; private set; }

        // Tổng số phần tử đã sao chép qua mọi lần mở rộng.
        public long TotalCopied { get; private set; }

        public DynamicArray(int capacity = 4)
        {
            // Dung lượng âm vô nghĩa nên từ chối ngay khi tạo đối tượng.
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "capacity không được âm.");
            }

            // Cấp phát mảng nội bộ đúng bằng capacity yêu cầu; Count mặc định = 0 (chưa có phần tử).
            items = new T[capacity];
        }

        public void Add(T item)
        {
            // Bảo đảm còn ít nhất một ô trống; hết chỗ thì EnsureCapacity mở rộng (tốn O(n)).
            EnsureCapacity();

            // Ghi phần tử mới vào ô trống đầu tiên, tức vị trí Count.
            items[Count] = item;

            // Tăng Count SAU khi ghi: trước đó Count chính là chỉ số ô trống cần ghi vào.
            Count++;
        }

        private void EnsureCapacity()
        {
            // Còn chỗ (Count < Capacity) thì không làm gì: nhánh thường gặp nên Add là O(1).
            if (Count < items.Length) return;

            // Gấp đôi dung lượng; riêng dung lượng 0 phải đặt về 4 vì 0 * 2 vẫn bằng 0 và mảng sẽ không bao giờ lớn lên.
            int newCapacity = items.Length == 0 ? 4 : items.Length * 2;

            // Tạo mảng mới lớn hơn; mảng cũ vẫn còn tồn tại nên lúc này tốn thêm O(n) bộ nhớ tạm.
            T[] newItems = new T[newCapacity];

            // Chỉ sao chép đúng Count phần tử đang hợp lệ, giữ nguyên thứ tự (chỉ số i cũ = chỉ số i mới).
            for (int i = 0; i < Count; i++)
            {
                newItems[i] = items[i];
            }

            // Ghi lại số phần tử đã sao chép của lần mở rộng này.
            LastCopiedCount = Count;

            // Cộng dồn vào tổng để so với phân tích khấu hao.
            TotalCopied += Count;

            // Chuyển items sang mảng mới; mảng cũ không còn ai tham chiếu nên GC sẽ thu hồi.
            items = newItems;
        }

        public T this[int index]
        {
            get
            {
                // Chỉ cho đọc phần tử hợp lệ (0..Count-1); các ô trống phía sau chưa có dữ liệu thật.
                CheckIndex(index);

                // Truy cập mảng theo chỉ số trực tiếp nên O(1).
                return items[index];
            }
            set
            {
                // Chỉ cho ghi đè phần tử đã tồn tại; thêm mới phải dùng Add để Count được cập nhật.
                CheckIndex(index);
                items[index] = value;
            }
        }

        // Dùng chung cho get và set để hai nơi kiểm tra giống hệt nhau.
        private void CheckIndex(int index)
        {
            // So với Count chứ KHÔNG so với Capacity: index == Count vẫn là ô trống, không phải phần tử hợp lệ.
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "index phải nằm trong đoạn từ 0 đến Count - 1.");
            }
        }
    }

    internal class Program
    {
        // In nội dung hợp lệ của mảng động qua indexer.
        static string Show(DynamicArray<int> arr)
        {
            // Dùng chuỗi nối đơn giản vì đây chỉ là mã minh họa với vài phần tử.
            string s = "[";
            for (int i = 0; i < arr.Count; i++) s += arr[i] + (i < arr.Count - 1 ? ", " : "");
            return s + "]";
        }

        // Mô phỏng chi phí sao chép tổng cộng khi thêm n phần tử, bắt đầu từ dung lượng 4.
        // doubling = true: gấp đôi; false: tăng thêm 1 mỗi lần hết chỗ.
        static long SimulateCopies(int n, bool doubling)
        {
            // Dung lượng hiện tại và số phần tử hiện có.
            int capacity = 4;
            int count = 0;

            // Tổng số lần sao chép phần tử (dùng long để không tràn khi n lớn).
            long copies = 0;

            // Mô phỏng n lần Add.
            for (int k = 0; k < n; k++)
            {
                // Hết chỗ thì phải mở rộng và sao chép toàn bộ count phần tử hiện có.
                if (count == capacity)
                {
                    copies += count;
                    capacity = doubling ? capacity * 2 : capacity + 1;
                }

                // Thêm phần tử mới.
                count++;
            }

            return copies;
        }

        static void Main()
        {
            // UTF-8 để hiển thị tiếng Việt.
            Console.OutputEncoding = Encoding.UTF8;

            // ---------- Phần 1: thêm 1..10 và theo dõi Count/Capacity ----------
            Console.WriteLine("=== Phần 1: Add 1..10, theo dõi Count và Capacity ===");
            DynamicArray<int> arr = new DynamicArray<int>();
            Console.WriteLine("Khởi tạo: Count=" + arr.Count + ", Capacity=" + arr.Capacity);

            for (int v = 1; v <= 10; v++)
            {
                // Nhớ Capacity trước khi Add để nhận biết lần Add này có làm mảng mở rộng hay không.
                int oldCapacity = arr.Capacity;
                arr.Add(v);

                string line = "Add(" + v + "): Count=" + arr.Count + ", Capacity=" + arr.Capacity;

                // Capacity đổi nghĩa là vừa mở rộng; LastCopiedCount cho biết số phần tử đã sao chép.
                if (arr.Capacity != oldCapacity)
                {
                    line += "   <-- mở rộng " + oldCapacity + " -> " + arr.Capacity + ", sao chép " + arr.LastCopiedCount + " phần tử";
                }
                Console.WriteLine(line);
            }
            Console.WriteLine("Tổng số phần tử đã sao chép qua mọi lần mở rộng: " + arr.TotalCopied + " (với n = 10 phần tử)");
            Console.WriteLine("Nội dung: " + Show(arr));

            // ---------- Phần 2: indexer ----------
            Console.WriteLine();
            Console.WriteLine("=== Phần 2: đọc/ghi qua indexer ===");
            Console.WriteLine("arr[2] = " + arr[2]);

            // Ghi đè phần tử số 2 bằng 300; Count không đổi vì đây là sửa chứ không phải thêm.
            arr[2] = 300;
            Console.WriteLine("Sau arr[2] = 300: " + Show(arr) + ", Count=" + arr.Count);

            try { Console.WriteLine(arr[-1]); }
            catch (ArgumentOutOfRangeException) { Console.WriteLine("arr[-1] -> ArgumentOutOfRangeException (đúng)"); }

            // index == Count (10) tuy vẫn nằm trong Capacity (16) nhưng chưa có dữ liệu nên phải bị từ chối.
            try { Console.WriteLine(arr[arr.Count]); }
            catch (ArgumentOutOfRangeException) { Console.WriteLine("arr[Count] -> ArgumentOutOfRangeException (đúng, Capacity=" + arr.Capacity + " nhưng Count=" + arr.Count + ")"); }

            // Ghi vào index == Count cũng bị chặn.
            try { arr[arr.Count] = 1; }
            catch (ArgumentOutOfRangeException) { Console.WriteLine("arr[Count] = 1 -> ArgumentOutOfRangeException (đúng)"); }

            // Capacity âm bị từ chối.
            try { DynamicArray<int> bad = new DynamicArray<int>(-1); }
            catch (ArgumentOutOfRangeException) { Console.WriteLine("new DynamicArray(-1) -> ArgumentOutOfRangeException (đúng)"); }

            // ---------- Phần 3: dữ liệu cũ giữ nguyên thứ tự khi 4 -> 8 ----------
            Console.WriteLine();
            Console.WriteLine("=== Phần 3: kiểm chứng lần mở rộng 4 -> 8 ===");
            DynamicArray<int> small = new DynamicArray<int>();
            small.Add(10); small.Add(20); small.Add(30); small.Add(40);
            Console.WriteLine("Trước: " + Show(small) + "  Count=" + small.Count + "  Capacity=" + small.Capacity);

            // Phần tử thứ 5 buộc mảng phải mở rộng từ 4 lên 8.
            small.Add(50);
            Console.WriteLine("Sau:   " + Show(small) + "  Count=" + small.Count + "  Capacity=" + small.Capacity + "  (sao chép " + small.LastCopiedCount + " phần tử)");
            Console.WriteLine("-> Bốn phần tử cũ 10,20,30,40 vẫn đúng thứ tự và đúng chỉ số.");

            // ---------- Phần 4: gấp đôi so với cộng thêm 1 ----------
            Console.WriteLine();
            Console.WriteLine("=== Phần 4: tổng số phần tử phải sao chép khi thêm n phần tử ===");
            int[] sizes = { 10, 100, 1000, 10000 };
            for (int i = 0; i < sizes.Length; i++)
            {
                int n = sizes[i];
                Console.WriteLine("n = " + n.ToString().PadRight(6) + " | gấp đôi: " + SimulateCopies(n, true).ToString().PadLeft(10) + " | tăng thêm 1: " + SimulateCopies(n, false).ToString().PadLeft(12));
            }
        }
    }
}
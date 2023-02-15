using System;

namespace Module_7_7
{
    // Моделль фрейма "Заказы" в интернет-магазине
    class MyOrders
    {
        // Закрытое поле, хранящее заказы в виде массива
        private Order<Delivery, Purshase, Bonus>[] collection;

        // Конструктор с добавлением массива заказов
        public MyOrders(Order<Delivery, Purshase, Bonus>[] collection)
        {
            this.collection = collection;
        }

        // Индексатор по массиву
        public Order<Delivery, Purshase, Bonus> this[int index]
        {
            get
            {
                // Проверяем, чтобы индекс был в диапазоне для массива
                if (index >= 0 && index < collection.Length)
                {
                    return collection[index];
                }
                else
                {
                    return null;
                }
            }
            private set
            {
                // Проверяем, чтобы индекс был в диапазоне для массива
                if (index >= 0 && index < collection.Length)
                {
                    collection[index] = value;
                }
            }
        }
    }

    // Класс для покупки
    class Purshase
    {
        private string name;
        private double price;
        private double weight;
        
        // Свойство наименование товара
        public string Name { get; set; }

        // Свойство цена товара только для чтения
        public double Price { get; }

        // Свойство вес товара только для чтения
        public double Weight { get; }

        // Конструктор с одним параметром
        public Purshase(string name)
        {
            Name = name;
            GetPurshaseProperties(Name);
        }

        // Метод определяющий цену и вес товара
        private void GetPurshaseProperties(string name)
        {
            switch (name)
            {
                case "Чехол":
                    price = 500;
                    weight = 0.1;
                    break;
                case "Наушники":
                    price = 1000;
                    weight = 0.3;
                    break;
                case "Смартфон":
                    price = 20000;
                    weight = 0.5;
                    break;
            }
        }
    }

    abstract class Delivery
    {
        private string address;
        // Стоимость доставки по умолчанию (до пункта выдачи или постамата)
        private double price;
        // Описание доставки
        private string description;
        // Дата доставки
        private DateTime date;

        // Свойство адрес
        public string Address { get; set; }

        // Свойство цена доставки
        public double Price { get; set; }

        // Свойство дата доставки
        public DateTime Date{ get; set; }

        // Свойство описание доставки
        public string Description { get; set; }

        // Метод отображающий тип доставвки
        public void DisplayDescription()
        {
            Console.WriteLine(description);
        }

        // Абстрактный метод для получения описания доставки
        public abstract string GetDeliveryDescription();

        // Виртуальный метод для рассчета цены доставки по умолчанию (до пункта выдачи или постамата, на дом будет дороже)
        // Будет переопределен в классе для доставки на дом
        protected virtual double GetDeliveryPrice()
        {
            double deliveryPrice = 10;
            return deliveryPrice;
        }
    }

    // Доставка на дом
    class HomeDelivery : Delivery
    {
        // Конструктор
        public HomeDelivery()
        {
            Description = GetDeliveryDescription();
            // Доставка на дом дольше на 2 дня, чем в пункт выдачи
            Date = DateTime.Today.AddDays(5);
            Price = GetDeliveryPrice();
        }

        // Переопределяем абстрактный метод
        public override string GetDeliveryDescription()
        {
            string descr = "Доставка на дом";
            return descr;
        }

        // Переопределяем только для этого класса виртуальный метод для рассчета цены доставки (на 5 рублей дороже)
        protected override double GetDeliveryPrice()
        {
            double deliveryPrice = 15;
            return deliveryPrice;
        }
    }

    // Доставка в постамат
    class PickPointDelivery : Delivery
    {
        // Конструктор
        public PickPointDelivery()
        {
            Description = GetDeliveryDescription();
            // Доставка в постамат дольше на 1 день, чем в пункт выдачи
            Date = DateTime.Today.AddDays(4);
            Price = GetDeliveryPrice();
        }

        // Переопределяем абстрактный метод
        public override string GetDeliveryDescription()
        {
            string descr = "Доставка в постамат";
            return descr;
        }
    }

    // Доставка в пункт выдачи
    class ShopDelivery : Delivery
    {
        // Конструктор
        public ShopDelivery()
        {
            Description = GetDeliveryDescription();
            // Доставка в пункт выдачи самая быстрая
            Date = DateTime.Today.AddDays(3);
            Price = GetDeliveryPrice();
        }

        // Переопределяем абстрактный метод
        public override string GetDeliveryDescription()
        {
            string descr = "Доставка в пункт выдачи";
            return descr;
        }
    }
    
    // Транзакция оплаты покупки - статический класс, со статическим полем
    static class Transaction
    {
        // % НДС
        public static int NDS = 10;

        public static void DisplayTransactionStatus()
        {
            Console.WriteLine("Заказ оплачен");
        }
    }

    // Электронный чек
    class Receipt
    {
        public double TotalAmount;
        public double NDSAmount;
    }

    // Бонусы
    class Bonus
    {
        // Количество бонусов, соотносятся с рублями 2/1
        public int BonusCount = 100;

        // Перегрузка оператора вычитания, чтобы вычесть бонусы из цены покупки
        public static double operator -(double a, Bonus b) => a + (-1 / 2 * b.BonusCount);

    }

    // Заказ
    class Order<TDelivery, TPurshase, TBonus>
        where TDelivery : Delivery
        where TPurshase : Purshase
        where TBonus : Bonus
    {
        public TDelivery Delivery;
        public TPurshase Purshase;
        public TBonus Bonus;

        private int number;

        private string description;

        // Свойство номер заказа только для записи
        public int Number { get; set; }

        // Свойство описание доставки только для чтения
        public string Description { get; }

        public Order(TDelivery delivery, TPurshase purshase, TBonus bonus)
        {
            Delivery = delivery;
            Purshase = purshase;
            Bonus = bonus;
            // Производим транзакцию
            MakeTransaction();
            Description = CreateDescription();
        }

        // Совершает транзакцию по оплате покупки, формирует электронный чек и выводит статус оплаты
        public void MakeTransaction()
        {
            Receipt receipt = new Receipt();
            // Рассчитываем итог по чеку за вычетом бонусов
            receipt.TotalAmount = Purshase.Price + Delivery.Price - Bonus;
            // Рассчитываем размер НДС (условно), используя статическое свойство из статического класса
            receipt.NDSAmount = receipt.TotalAmount * Transaction.NDS / 100;
            Transaction.DisplayTransactionStatus();
        }

        // Создаем описание заказа
        private string CreateDescription()
        {
            string descr = "Заказ от " + DateTime.Today.Date + "\n" + "Номер " + Number + "\n" + Delivery.Description + "\n" + "Ожидаемая дата доставки " + Delivery.Date + "\n";
            return descr;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Создаем массив заказов
            var array = new Order<Delivery, Purshase, Bonus>[3];

            // Создаем экземпляр класса Bonus
            Bonus bonus = new Bonus();

            // Заполняем массив
            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine("Выберите номер товара: 1 - Чехол, 2 - Наушники, 3 - Смартфон");
                int purshaseNum = int.Parse(Console.ReadLine());
                string purshaseName;
                switch (purshaseNum)
                {
                    case 1:
                        purshaseName = "Чехол";
                        break;
                    case 2:
                        purshaseName = "Наушники";
                        break;
                    case 3:
                        purshaseName = "Смартфон";
                        break;
                    default:
                        purshaseName = "Смартфон";
                        break;
                }

                Purshase purshase = new Purshase(purshaseName);

                Console.WriteLine("Выберите номер доставки: 1 - На дом, 2 - В постамат, 3 - В пункт выдачи");
                int deliveryNum = int.Parse(Console.ReadLine());
                Console.WriteLine("Укажите адрес, постамат или пункт выдачи");
                string deliveryAddress = Console.ReadLine();

                switch (deliveryNum)
                {
                    case 1:
                        HomeDelivery homeDelivery = new HomeDelivery();
                        homeDelivery.Address = deliveryAddress;
                        array[i] = new Order<Delivery, Purshase, Bonus>(homeDelivery, purshase, bonus);
                        break;
                    case 2:
                        PickPointDelivery pickpointDelivery = new PickPointDelivery();
                        pickpointDelivery.Address = deliveryAddress;
                        array[i] = new Order<Delivery, Purshase, Bonus>(pickpointDelivery, purshase, bonus);
                        break;
                    case 3:
                        ShopDelivery shopDelivery = new ShopDelivery();
                        shopDelivery.Address = deliveryAddress;
                        array[i] = new Order<Delivery, Purshase, Bonus>(shopDelivery, purshase, bonus);
                        break;
                    default:
                        Console.WriteLine("Вы ввели неправильный номер доставки.");
                        break;
                }

                array[i].Number = i + 1;
            }

            // Создаем коллекцию с массивами заказов
            MyOrders collection = new MyOrders(array);

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(collection[i].Description);
            }
        }
    }
}

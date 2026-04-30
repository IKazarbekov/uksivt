Student[] students = new Student[5];
students[0] = new Student("Tom", new int[] { 2, 6, 2, 6, 2, 6, 4, 3 });
students[1] = new Student("Rey", new int[] { 1, 1 });
students[2] = new Student("Lag", new int[] { 3, 5, 3, 7, 5, 74, 65, 74, 5, 7, 7 });
students[3] = new Student("Dog", new int[] { 3, 6, 2, 6, 4 });
students[4] = new Student("Lazy", new int[] { 3, 7, 2, 7, 4, 65 });
GradeAnalyzer.staticSortByGPA(students);
Console.WriteLine("Все студенты");
foreach(Student student1 in students)
{
    Console.WriteLine($"{student1.name} - GPA: {student1.GPA}");
}
Console.WriteLine("Все студенты c GPA >= 4");
Student[] studentMax = GradeAnalyzer.FilterByGPA(students, 4);
foreach (Student student in studentMax)
{
    Console.WriteLine($"{student.name} - GPA: {student.GPA}"); 
}

class Student : IComparable<Student>
{
    public string name;
    public int GPA
    {
        get
        {
            return GradeAnalyzer.CalculateAverage(grades);
        }
    }
    public int[] grades;

    public Student(string name, int[] Grades)
    {
        this.name = name;
        this.grades = Grades;
    }

    public int CompareTo(Student other)
    {
        return GPA.CompareTo(other. GPA );
    }
}

static class GradeAnalyzer
{
    public static int CalculateAverage(int[] grades)
    {
        int summa = 0;
        foreach (int a in grades)
        {
            summa += a;
        }
        return summa / grades.Length;
    }
    public static int FindMax(int[] grades)
    {
        int max = int.MinValue;
        foreach (int a in grades)
        {
            if (max < a)
            {
                max = a;
            }
        }
        return max;
    }
    public static int FindMin(int[] grades)
    {
        int min = int.MaxValue;
        foreach (int a in grades)
        {
            if (min > a)
            {
                min = a;
            }
        }
        return min;
    }

    public static Student[] FilterByGPA(Student[] students, double minGPA)
    {
        List<Student> list = new List<Student>();
        foreach (Student student in students)
        {
            if (student.GPA >= minGPA)
            {
                list.Add(student);
            }
        }
        return list.ToArray();
    }
    public static void staticSortByGPA(Student[] students)
    {
        Array.Sort(students);
    }

}


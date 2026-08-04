using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Program
{
    static void Main(string[] args)
    {
        LoadData(out List<string> questions, out List<int> answers);

        bool isRunning = true;

        while (isRunning)
        {
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1. Добавить вопрос в форму");
            Console.WriteLine("2. Пройти опрос (ввести данные)");
            Console.WriteLine("3. Посмотреть результаты");
            Console.WriteLine("4. Выход");
            Console.Write("Выберите пункт: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Раздел добавления вопросов");
                    Console.WriteLine("\n[Добавление вопроса]");
                    Console.Write("Введите вопрос: ");
                    string NewQuestion = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(NewQuestion))
                    {
                        questions.Add(NewQuestion);
                        SaveData(questions, answers);
                        Console.WriteLine("Вопрос успешно добавлен и сохранен.");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: Вопрос не может быть пустым.");
                    }

                    break;

                case "2":
                    Console.WriteLine("Раздел прохождения опроса");
                    Console.WriteLine("\n[Прохождение опроса]");

                    if (questions.Count == 0)
                    {
                        Console.WriteLine("Нет доступных вопросов для опроса. Пожалуйста, добавьте вопросы сначала.");
                        break;
                    }

                    answers.Clear();

                    foreach (var question in questions)
                    {
                        while (true)
                        {
                            Console.WriteLine(question);
                            Console.Write("Введите ваш ответ (число): ");
                            string input = Console.ReadLine();
                            if (int.TryParse(input, out int answer))
                            {
                                answers.Add(answer);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Ошибка: Пожалуйста, введите корректное число.");
                            }
                        }
                    }

                    SaveData(questions, answers);
                    Console.WriteLine("Опрос завершен. Ваши ответы сохранены.");
                    break;

                case "3":
                    Console.WriteLine("Раздел результатов");
                    Console.WriteLine("\n=== РЕЗУЛЬТАТЫ ===");

                    if (answers.Count == 0)
                    {
                        Console.WriteLine("Нет данных для отображения. Пожалуйста, пройдите опрос сначала.");
                        break;
                    }

                    int sum = 0;

                    for (int i = 0; i < questions.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {questions[i]}: {answers[i]}");
                        sum += answers[i];
                    }
                    double avg = (double)sum / answers.Count;

                    Console.WriteLine("-----------------------");
                    Console.WriteLine($"Всего ответов: {answers.Count}");
                    Console.WriteLine($"Сумма: {sum}");
                    Console.WriteLine($"Среднее значение: {avg:F2}");

                    break;

                case "4":
                    Console.WriteLine("Завершение работы...");
                    return;

                default:
                    Console.WriteLine("Неверный ввод. Пожалуйста, выберите пункт от 1 до 4.");
                    break;
            }
        }
    }


    static void SaveData(List<string> questions, List<int> answers)
    {
        string questionsJson = JsonSerializer.Serialize(questions);
        string answersJson = JsonSerializer.Serialize(answers);

        File.WriteAllLines("data.json", new string[] { questionsJson, answersJson });
    }

    static void LoadData(out List<string> questions, out List<int> answers)
    {
        questions = new List<string>();
        answers = new List<int>();

        if (File.Exists("data.json"))
        {
            string[] lines = File.ReadAllLines("data.json");
            if (lines.Length >= 2)
            {
                questions = JsonSerializer.Deserialize<List<string>>(lines[0]) ?? new List<string>();
                answers = JsonSerializer.Deserialize<List<int>>(lines[1]) ?? new List<int>();
            }
        }
    }
}
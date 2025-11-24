# Driver Matching System

Система подбора водителей на заказ. Реализованы 3 алгоритма:
- Brute Force
- KD-Tree
- Grid Bucket (Spatial Hashing)

## Как запустить тесты
1. Откройте `Тест → Окно тестов`
2. Нажмите **Запустить все**

## Как запустить бенчмарк
1. Переключитесь в конфигурацию **Release**
2. Сделайте `DriverMatching.Benchmarks` стартовым проектом (ПКМ → Назначить запускаемым)
3. Нажмите `Ctrl+F5`

Результаты будут в консоли и в `BenchmarkDotNet.Artifacts/results/`.

## Пример вывода (10 000 водителей)

| Method      |     Mean | Rank |
|-------------|---------:|-----:|
| KDTree      |  44.1 μs |    1 |
| GridBucket  |  58.3 μs |    2 |
| BruteForce  | 1.098 ms |    3 |
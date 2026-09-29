# AlgoLab

**AlgoLab** — платформа для бенчмаркинга алгоритмов с визуализацией результатов. Позволяет запускать замеры алгоритмов на различных диапазонах входных данных, сравнивать их между собой, отслеживать сложность через аппроксимацию и строить 2D/3D-графики.

---

## Содержание

- [Возможности](#возможности)
- [Стек технологий](#стек-технологий)
- [Архитектура](#архитектура)
- [Структура проекта](#структура-проекта)
- [Требования](#требования)
- [Запуск](#запуск)
- [API](#api)
- [Как пользоваться](#как-пользоваться)
- [Особенности реализации](#особенности-реализации)

---

## Возможности

- 📊 **Замер алгоритмов** по времени (`Stopwatch`) или по количеству шагов.
- 🗂 **Выбор диапазона** — начальное и конечное N, шаг; для 2D-алгоритмов дополнительно M.
- ⚡ **Кэширование результатов** — повторные замеры берутся из БД, если не выставлен флаг принудительного пересчёта.
- 🔄 **Асинхронный запуск** — сессия уходит в очередь и выполняется в фоне, UI обновляется автоматически.
- ❌ **Отмена длительных замеров** через `CancellationToken`.
- 📈 **Аппроксимация сложности** — подбор лучшей модели из O(1), O(log n), O(n), O(n log n), O(n²), O(n³) методом наименьших квадратов.
- 🎯 **Фильтрация выбросов** методом межквартильного размаха (IQR).
- 🧊 **3D-поверхность** для двумерных алгоритмов (умножение матриц).
- 📉 **Сравнение нескольких сессий** на одном графике.

---

## Стек технологий

### Backend

| Слой | Технология | Версия |
|---|---|---|
| Платформа | .NET | 10.0 |
| Web | ASP.NET Core | 10.0 |
| ORM | Entity Framework Core | 10.0.4 |
| БД | PostgreSQL (Npgsql) | 10.0.3 |
| Swagger | Swashbuckle.AspNetCore | 10.2.3 |
| Фон | `BackgroundService` + `System.Threading.Channels` | встроено |

### Frontend

| Слой | Технология | Версия |
|---|---|---|
| UI | React | 19.2 / 19.3 |
| Сборка | Vite (на Rolldown) | 8.3.0 |
| Данные | TanStack Query | 5.103.2 |
| HTTP | Axios | — |
| Графики | Plotly.js (`plotly.js-dist-min`) | 4.1.1 |
| Линтер | oxlint | 1.85 |

---

## Архитектура

Backend построен по **луковичной (Onion / Clean) архитектуре** — зависимости направлены внутрь, домен не знает ни о чём:

```
┌──────────────────────────────────────────────┐
│  Presentation  — ASP.NET Core (Controllers)  │
├──────────────────────────────────────────────┤
│  Infrastructure — EF Core, benchmark engine  │
├──────────────────────────────────────────────┤
│  Algorithms    — реализации алгоритмов       │
├──────────────────────────────────────────────┤
│  Application   — handlers, interfaces, DTO   │
├──────────────────────────────────────────────┤
│  Domain        — entities, enums, models     │
└──────────────────────────────────────────────┘
```

**Поток выполнения бенчмарка:**

```
POST /api/benchmarks
   ↓
StartBenchmarkHandler: валидация → создание BenchmarkSession → очередь
   ↓ (ответ 201 + sessionId)
BenchmarkQueue: Channel<Guid>
   ↓
BenchmarkWorker (BackgroundService)
   ↓
BenchmarkExecutor
   ├─ для каждого N (и M):
   │    ├─ проверка кэша (BenchmarkRuns)
   │    └─ при промахе — BenchmarkRunner.MeasureAsync
   ├─ IQR-фильтр выбросов
   └─ подбор модели аппроксимации
   ↓
SessionStatus = Completed
```

---

## Структура проекта

```
algo-lab/
├── src/
│   ├── Core/
│   │   ├── AlgoLab.Domain/          # Сущности, enum'ы, модели
│   │   │   ├── Entities/            # Algorithm, BenchmarkSession, BenchmarkRun, SessionRun
│   │   │   ├── Enums/               # SessionStatus
│   │   │   └── Models/              # Matrix, MatrixPair, PowInput
│   │   │
│   │   └── AlgoLab.Application/     # Use case'ы, интерфейсы, DTO
│   │       ├── Common/
│   │       │   ├── Interfaces/      # IAlgorithm, IDataGenerator, IBenchmarkQueue, ...
│   │       │   └── Models/          # BenchmarkResult, GenerationRequest
│   │       ├── DTOs/
│   │       └── Features/
│   │           ├── Algorithms/GetAlgorithms/
│   │           └── Benchmarks/
│   │               ├── StartBenchmark/
│   │               ├── CancelBenchmark/
│   │               ├── GetHistory/
│   │               ├── GetSessionDetails/
│   │               └── GetComparison/
│   │
│   ├── AlgoLab.Algorithms/          # Реализации алгоритмов
│   │   ├── AlgorithmRegistry.cs
│   │   └── Implementations/
│   │       ├── ConstFunction.cs
│   │       ├── SumFunction.cs
│   │       ├── ProductFunction.cs
│   │       ├── NaivePolynomial.cs
│   │       ├── HornerPolynomial.cs
│   │       ├── BubbleSort.cs
│   │       ├── QuickSort.cs
│   │       ├── TimSort.cs
│   │       ├── SmoothSort.cs
│   │       ├── SieveOfEratosthenes.cs
│   │       ├── AtkinSieve.cs
│   │       ├── MultiplyMatrix.cs
│   │       ├── SimplePow.cs
│   │       ├── RecursivePow.cs
│   │       └── QuickRecursivePow.cs
│   │
│   ├── Infrastructure/
│   │   └── AlgoLab.Infrastructure/
│   │       ├── Persistence/         # AppDbContext, configurations
│   │       ├── Migrations/          # EF Core миграции
│   │       ├── Benchmarking/        # Queue, Worker, Executor, Runner, Statistics
│   │       │   └── Generators/      # Генераторы входных данных
│   │       └── DependencyInjection.cs
│   │
│   └── Presentation/
│       └── AlgoLab.API/             # Web API
│           ├── Controllers/
│           ├── Middleware/
│           ├── Program.cs
│           └── appsettings.json
│
└── algo-lab-frontend/               # React SPA
    ├── src/
    │   ├── api/                     # Axios-клиент и запросы
    │   ├── components/              # UI-кит (Button, Input, Table, StatusBadge)
    │   ├── features/
    │   │   ├── benchmark-configuration/
    │   │   ├── benchmark-history/
    │   │   └── benchmark-visualization/
    │   ├── pages/
    │   │   └── VisualizationPage.jsx
    │   ├── providers/
    │   ├── App.jsx
    │   └── main.jsx
    ├── package.json
    └── vite.config.js
```

---

## Требования

- **.NET SDK 10.0** или новее — [скачать](https://dotnet.microsoft.com/download)
- **Node.js 20.19+** или **22.12+** — [скачать](https://nodejs.org/)
- **PostgreSQL 14+** — [скачать](https://www.postgresql.org/download/) или через Docker
- **Git**
- **EF Core CLI** (для миграций):
  ```bash
  dotnet tool install --global dotnet-ef
  ```

---

## Запуск

### 1. PostgreSQL

 Создайте БД:

```sql
CREATE DATABASE algolab;
```

Строка подключения по умолчанию (в `appsettings.json`):

```
Host=localhost;Port=5432;Database=algolab;Username=postgres;Password=postgres
```

Если ваши креды отличаются — отредактируйте `src/Presentation/AlgoLab.API/appsettings.json`.

```bash
# Из корня репозитория
cd src/Presentation/AlgoLab.API

# Применяем миграции (создаст таблицы и засеет 15 алгоритмов)
dotnet ef database update --project ../../Infrastructure/AlgoLab.Infrastructure
```

### 2. Запуск проекта

Запустите скрипт `run.bat` для запуска проекта.

API поднимется на **http://localhost:5163**. Swagger — **http://localhost:5163/swagger** (доступен в Development).
Чтобы открыть сайт, зайдите на **http://localhost:5173** (Vite по умолчанию).
---

## API

| Метод | Путь | Назначение |
|---|---|---|
| `GET` | `/api/algorithms` | Список алгоритмов |
| `POST` | `/api/benchmarks` | Запустить сессию бенчмарка |
| `GET` | `/api/benchmarks` | История сессий |
| `GET` | `/api/benchmarks/{id}` | Детали сессии (точки + аппроксимация) |
| `POST` | `/api/benchmarks/{id}/cancel` | Отменить активную сессию |
| `GET` | `/api/benchmarks/comparison?sessionIds=...` | Данные для сравнительного графика |

---

## Как пользоваться

1. **Выберите алгоритмы** — отметьте один или несколько в разделе «1. Выбор алгоритмов».
2. **Настройте параметры** — для каждого выбранного алгоритма задайте диапазон N (для матричного умножения — ещё и M) и шаг.
3. **Запустите замер** — нажмите «Запустить замер». Сессия появится в разделе «3. История замеров» со статусом `Pending` → `Running` → `Completed`.
4. **Откройте визуализацию** — нажмите «Открыть» в строке завершённой сессии. Появятся:
   - график зависимости времени/шагов от N;
   - линия аппроксимации с указанием модели (например, `O(n log n)`);
   - для 2D-алгоритмов — 3D-поверхность;
   - таблица замеров.
5. **Сравните алгоритмы** — отметьте чекбоксы в блоке «Сравнение алгоритмов».

---

## Особенности реализации

### Временные и шаговые алгоритмы

Интерфейсы разделены:

```csharp
public interface IAlgorithm<TInput> { void Execute(TInput input); }
public interface IStepAlgorithm<TInput> { long ExecuteCountingSteps(TInput input); }
```

Для `IAlgorithm<T>` замеряется `Stopwatch`, для `IStepAlgorithm<T>` — считается количество операций. Это позволяет сравнивать «чистую» сложность независимо от JIT/GC.

### Кэширование

Замеры хранятся в `benchmark_runs` с уникальным ключом `(AlgorithmId, N, M)`. Повторный запуск с теми же параметрами не пересчитывает точки. Флаг `ForceRecalculate` в запросе форсирует пересчёт.

### Асинхронность

Запуск сессии мгновенно возвращает `sessionId`, работа идёт в фоне. Фронт опрашивает `/api/benchmarks` каждые 2 секунды, пока есть активные сессии.

### Фильтрация выбросов

После каждого прохода по точкам применяется IQR-фильтр (фактор 1.5). Точки-выбросы помечаются флагом `IsOutlier` и исключаются из аппроксимации.

---

### Добавление нового алгоритма

1. Создайте класс в `src/AlgoLab.Algorithms/Implementations/`, реализующий `IAlgorithm<T>` или `IStepAlgorithm<T>`.
2. Зарегистрируйте его в `DependencyInjection.AddAlgorithms()`.
3. Добавьте запись в seed-данные (`AppDbContext.OnModelCreating`) и создайте миграцию.

---

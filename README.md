# Advanced ESP (Glow Plugin)

> **Примечание:** Этот плагин был разработан давно для Counter-Strike 2 сервера. Публикую сейчас для сообщества.

Продвинутый плагин подсветки игроков (ESP/Glow) для Counter-Strike 2 серверов на базе CounterStrikeSharp.

## 🎯 Возможности

- **Несколько режимов свечения:**
  - `all` - Все игроки
  - `enemies` - Только враги
  - `team` - Только союзники
  - `sound` - Враги, которые производят шум (стрельба, бег, прыжки и т.д.)
  - `visible` - Только видимые враги (на радаре)

- **Гибкая настройка:**
  - Настраиваемые цвета для каждой команды
  - Регулируемая дальность и стиль свечения
  - Настройка параметров звукового обнаружения

- **Система прав доступа:**
  - Интеграция с IksAdminApi
  - Настраиваемый флаг доступа

- **Discord логирование:**
  - Отправка логов использования команд в Discord
  - Красивые embed-сообщения с информацией о действиях администраторов

## 📋 Требования

- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp) (latest version)
- [IksAdminApi](https://github.com/Iksix/Iks_Admin) для системы прав доступа

## 🔧 Установка

1. Установите CounterStrikeSharp на ваш сервер
2. Установите IksAdminApi
3. Скопируйте файлы плагина в папку `addons/counterstrikesharp/plugins/AdvancedGlow/`
4. Перезапустите сервер или загрузите плагин командой `css_plugins load AdvancedGlow`

## ⚙️ Конфигурация

После первого запуска в папке `addons/counterstrikesharp/configs/plugins/AdvancedGlow/` будет создан файл конфигурации:

```json
{
  "Command": "esp",
  "EspAccessFlag": "e",
  "GlowSettings": {
    "GlowStyle": 3,
    "GlowRange": 5000,
    "SoundGlowMinSpeed": 150.0,
    "SoundGlowMaxDistance": 1100.0
  },
  "ColorSettings": {
    "DefaultTerroristColor": "255, 50, 50, 220",
    "DefaultCounterTerroristColor": "50, 150, 255, 220"
  },
  "DiscordSettings": {
    "Enabled": false,
    "WebhookUrl": "PASTE_YOUR_DISCORD_WEBHOOK_URL_HERE",
    "EmbedTitle": "Advanced ESP Logs"
  }
}
```

### Параметры конфигурации:

- **Command** - команда для переключения режимов свечения
- **EspAccessFlag** - флаг доступа к команде (по умолчанию "e")
- **GlowStyle** - стиль свечения (0-3)
- **GlowRange** - дальность видимости свечения
- **SoundGlowMinSpeed** - минимальная скорость для обнаружения в режиме "sound"
- **SoundGlowMaxDistance** - максимальная дистанция обнаружения звука
- **DefaultTerroristColor** - цвет свечения террористов (R, G, B, A)
- **DefaultCounterTerroristColor** - цвет свечения контр-террористов (R, G, B, A)

### Discord интеграция:

1. Создайте webhook в вашем Discord канале
2. Вставьте URL в параметр `WebhookUrl`
3. Установите `Enabled: true`

## 🎮 Использование

### Команды

```
!esp <режим>
```

Доступные режимы:
- `all` - показать всех игроков
- `enemies` - показать только врагов
- `team` - показать только союзников
- `sound` - показать шумящих врагов
- `visible` - показать видимых врагов (на радаре)

Повторный ввод той же команды отключает режим.

### Примеры

```
!esp enemies  - включить подсветку врагов
!esp enemies  - отключить подсветку (повторный ввод)
!esp sound    - включить подсветку шумящих врагов
!esp all      - включить подсветку всех игроков
```

## 🔐 Права доступа

По умолчанию для использования команды требуется флаг `e` или `z` (root). 

Настройте права через IksAdmin:
```
css_am_add <steamId> <name> <time/0> <server_id/this/all> <flags> <immunity>
```

Где флаг `e` дает доступ к ESP функциям.

## 🏗️ Структура проекта

```
AdvancedGlow/
├── AdvancedGlow.cs              # Главный файл плагина
├── Config.cs                     # Конфигурация
├── Models/
│   ├── GlowEntitySet.cs         # Модель сущностей свечения
│   └── DiscordWebhookModels.cs  # Модели для Discord
├── Services/
│   ├── GlowManager.cs           # Управление свечением
│   ├── GlowEntityService.cs     # Создание/удаление сущностей
│   └── GlowVisibilityService.cs # Логика видимости
└── Utils/
    ├── ChatUtils.cs             # Утилиты для чата
    └── DiscordLogger.cs         # Логирование в Discord
```

## 🤝 Вклад в проект

Приветствуются pull requests и предложения по улучшению плагина!

## 📝 Лицензия

Этот проект распространяется свободно для использования и модификации.

## 👤 Автор

**ALBAN-1776**

**Версия:** 1.1.0  
**Совместимость:** CounterStrikeSharp (latest)

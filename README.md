# Dungemon
Project1 for SE341: 2D-game development
## 操作指南

### 基本键位
* `↑↓←→`：控制主角移动方向
* `J`：普通攻击
* `K`：使用技能——男生：滑板车/女生：发射光球
* `Z`: 使用炸弹，原地放置，2s后爆炸
* `WASD`：控制伙伴移动方向
* `Alpha1，2，3`：命令伙伴使用技能

### 游戏流程

进入游戏最先显示的是选择界面UI
```shell
在输入栏输入玩家昵称，回车后代表确定
下拉菜单可以选择主角性别，男女角色在游戏玩法上会有一定程度上的不同，可根据喜好自行选择
点击Start开始游戏
```
开始游戏后，游戏会随机生成十个地牢，地牢主要分为六个类型
* `初始地牢`：主角初始生成的地方，什么都没有
* `普通地牢`：只会生成小怪（青蛙）的地牢，一般有1-4个
* `头目地牢`：地板边框为绿色的地牢，有头目存在，数目固定为四个，每个头目都不同
* `宝藏地牢`：没有怪物的地牢，地上只有零散的道具，供玩家休息补给
* `最终地牢`：地板边框为红色的地牢，一般离初始地牢很远且只有一个。会生成最终boss火龙，玩家击败后游戏通关。解锁的同时房间内会随机生成道具，帮助玩家战胜boss。

游戏进行时可以通过侧边UI查看主角状态，侧边UI包括
* `头像`：主角的头像，以性别为区分的角色图片
* `名字`：显示主角开始输入的昵称
* `状态`：在名字的下方，显示玩家目前的状态，状态有正常、灼烧、冷静、眩晕、死亡五种
* `技能图示`：显示玩家的技能，男女会有所区分，玩家使用后会有冷却动画显示冷却进度
* `生命值`：显示玩家当前的生命值，生命值小于0即为死亡

当玩家死亡或者击败最终boss后，游戏会进入结算界面，显示玩家的胜利与否和分数。玩家也可通过该界面退出游戏或者重开。

### 游戏内容

玩家技能介绍
* `普通攻击`：按J向前方挥拳攻击，对敌方造成**1点伤害**
* `滑板车`：
  ```shell
  男角色专有。按K进入骑行状态再按一次退出并进入冷却，冷却时间3s。
  骑行状态下速度大幅增加但只能水平移动，撞击敌人后对敌人造成**2点伤害**并退出骑行
  ```
* `光球`：女角色专有，向前发射光球，对敌人造成**2点伤害**，撞到墙壁自行消散，冷却时间2s
* `炸弹`：炸弹爆炸后对**半径2.5**内的敌人造成**高额伤害**（小怪会被直接炸死）

道具效果介绍
* `炸弹`：主角拾取后记炸弹数+1，使用过后数目-1
* `食物`：拾取后**生命值+3**
* `冷却宝石`：拾取后进入**冷静**状态，持续**3.5**，冷静状态下使用技能**冷却时间为0**
* `经验宝石`：拾取后增加伙伴的经验

道具可以通过宝藏房间或者击杀怪物获得，其中击杀头目掉落道具概率较高。

怪物介绍
| 怪物 | 生命值 | 移速 | 攻击伤害 | 特殊能力 | 描述 |
| :----: | :----: | :----: | :----: | :----: | :----: |
| 青蛙 | 3 | 5 | 1 | 无 | 普通小怪，很多，房间内小范围移动 |
| 哥布林 | 30 | 0 | 2 | 横向跳跃，击退 | 头目怪兽，虽然不会移动，但攻击范围很大，会横向跳跃 |
| 魔眼 | 30 | 7 | 2 | 四方飞弹，击退 | 头目怪兽，漂浮移动，会向四个方向发射飞弹 |
| 蘑菇怪 | 30 | 7 | 2 | 眩晕，击退 | 头目怪兽，向前拍击技能会使主角眩晕混乱 |
| 巨大骷髅 | 30 | 9 | 2 | 回复，击退 | 头目怪兽，拥有巨大的身躯和攻击范围，举盾的攻击会回复自身生命值 |
| 火龙 | 50 | 0 | 3-5 | 火球爆炸，灼烧，击退，伤害减免 | 最终boss，虽然不会移动，但拥有恐怖的攻击火力，攻击会让玩家灼烧，玩家靠近时会发射击退烈焰，同时玩家的技能造成伤害减半 |

状态介绍
* `冷静`：玩家技能冷却时间为0，持续3.5s
* `眩晕`：被蘑菇怪估计后会进入眩晕状态，**此时不能行动，且后续移动方向会进入混乱**，顺着击退方向移动可接触，**持续1.5s**
* `受伤无敌帧`：玩家在受伤后会进入短暂无敌期，免疫伤害，**持续0.8s**
* `灼烧`：被火龙的攻击命中后进入灼烧状态，**移速8->5**，灼烧状态下被火龙攻击会受到**更多的伤害**，**持续4.5s**
* 
### 游戏核心机制

进入游戏后游戏的十个地牢为**随机生成**，每个房间连接方式与通道数量皆为随机。玩家可以在十个地牢内自由穿行，自行选择击杀怪兽或者拾取道具。只有击败了四个头目以后，最终房间才会生成火龙。

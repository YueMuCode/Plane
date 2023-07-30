Plane项目开发记录

# 计划新加功能：

1、按住空格，玩家能够移动得更快，消耗耐力，耐力通过击杀敌人获得

2、增加冲撞型敌人





















Unity  inputSystem



## 飞机的旋转功能：

Quaternion.AngleAxis（）

协程

![image-20230714210247508](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230714210247508.png)

传入一个四元数（旋转的角度，绕什么轴旋转）

用lerp（要改变的值，目标值，插值（变化的快慢））在协程里面就可以实现了旋转的过场。



传参：

定义好一个初始的角度

![image-20230714210409420](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230714210409420.png)



当按下按键（按键的值如同UE4，从-1到1，不按为零）的时候，旋转的角度就会发生变化，方向写死

![image-20230714210503027](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230714210503027.png)











画面的光照（后处理效果）：

Volume脚本为URP包自带

![image-20230714221429627](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230714221429627.png)







## 子弹的设计

![image-20230714225209815](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230714225209815.png) 



子弹的移动 ：

![image-20230714230010919](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230714230010919.png)











## 4.玩家射击功能

![image-20230715111939283](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230715111939283.png)

## 子弹的销毁





## 子弹的管理--对象池

![image-20230715141339449](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230715141339449.png)

![image-20230715141959889](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230715141959889.png)



![image-20230715144611031](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230715144611031.png)





![image-20230715144722683](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230715144722683.png)

 

![image-20230715145308144](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230715145308144.png)

被框起来的这段代码，只会在编辑器的阶段运行，这意味着如果打包发布，将不会运行这段代码，方便调试。

多数用debug代码





## 6敌人

### 1.敌人的移动

### 2.随机出生点



## 7手动改变脚本执行的顺序

![image-20230715172959534](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230715172959534.png)





## 8.生命值系统

![image-20230715223354484](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230715223354484.png)

### 8.1自动回血









## 9.创建更多的敌人的子弹

### 9.1自动瞄准的自动（不是跟踪，而是朝玩家的方向发射）









## 10.触发器和物理碰撞

![image-20230716000357597](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230716000357597.png)

![image-20230716000505430](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230716000505430.png)

![image-20230716000528956](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230716000528956.png)

![image-20230716000659571](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230716000659571.png)

![image-20230716000941115](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230716000941115.png)

![image-20230716001007702](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230716001007702.png)





## 11.血条UI的射击

### 11.1血条缓冲的效果

### 11.2血条发光

### 11.3协程的重发开启问题





## 12.目前脚本的架构

![image-20230716112458584](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230716112458584.png)

## 13.能量系统

![image-20230716143209017](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230716143209017.png)

### 13.1常量的声明

![image-20230716143525062](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230716143525062.png)

### 13.2能量的增加

### 13.3能量的消耗

### 13.4翻滚

### 13.5缩放



## 14.敌人管理器

![image-20230716154835238](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230716154835238.png)

#### 14.2S生成敌人

![image-20230716155015276](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230716155015276.png)

### 14.3生成下一波的敌人

![image-20230716160807318](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230716160807318.png)

### 14.4WaitUntil





## 15.分析器

可以分析脚本运行的情况

![image-20230716232029668](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230716232029668.png)

## 16.波数UI

1、静态UI

2、动态UI用代码实现

3、动态UI用动画animator实现

![image-20230716234222412](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230716234222412.png)



## 17.持久泛型单例（不会随着场景的切换而销毁）



## 18.音效播放功能

![image-20230717235231326](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230717235231326.png)

![image-20230717235917225](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230717235917225.png)



![image-20230718000959424](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230718000959424.png)

每次调用这个函数，都会重新播放，导致声音又断续的感觉



解决办法：

![image-20230718001034576](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230718001034576.png)

不会覆盖



19场景管理器

19.1新建场景

19.2场景加载器





20分数系统

20.1分数的ui

20.2分数管理器



21能量的爆发



22追踪子弹的实现

22.1要用数学知识修改子弹的朝向从而实现子弹的追踪功能

![image-20230729234809068](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230729234809068.png)



bug的修复：玩家会移动出边框





23Unity的时间刻度

![image-20230730002956510](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230730002956510.png)



24、暂停菜单（UI）

24、1按钮的音效

![image-20230730134618741](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230730134618741.png) 



25游戏的状态设计

![image-20230730141609281](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230730141609281.png)



26、发射导弹

![image-20230730143802611](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230730143802611.png)



![image-20230730143830052](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230730143830052.png)



27、导弹爆炸导致的范围伤害



28、导弹的冷却时间





29、游戏主菜单

游戏的机制为STG



30、游戏死亡画面



31、分数排行榜（数据管理）





32、Boss战斗

32、1boss血条

32![image-20230730204540502](C:\Users\LinYueMU\AppData\Roaming\Typora\typora-user-images\image-20230730204540502.png)



33、枪口特效



34、boss波数



目前的bug：1、玩家不能移动

​					  2、枪口特效不能正常显示

​					  3、UI的显示也有点问题



# 二、系统

### 1.对象池系统： Pool.cs PoolManager.cs

Pool.cs生成对象池（根据传入的预制体和数量，生成对应的预制体）

PoolManager.cs管理生成的对象池（管理多个对象池（敌人对象池、子弹对象池等），的使用）


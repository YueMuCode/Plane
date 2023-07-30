Unity 需要学习的有：

1、

协程的详细过程

注意注意：协程不是默认自动持续执行的！是在里面加了循环语句！！！，协程一旦执行完所有逻辑就会自动停止



1.1协程的运行顺序是在update和lateupdate之间的。

1.2 yield return time表示挂起特定的时间后再回来执行下面的语句

1.3 yield return null 表示挂起，知道下一帧再执行接下来的语句



1.4以下面这段代码为例子，

IEnumerator RandomlySpawnCoroutine()
{
  A -enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / 3, maxEnemyAmount);
   for (int i = 0; i < enemyAmount; i++)
   {
     B-enemyList.Add( PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));
      C-yield return null;
   }

​	D-yield return waitTime;//waitTime是已经写好的waitforSecond类对象

   E-waveNumber++;
}

1.5当我们执行StartCoroutine（“RandomlySpawnCoroutine”）；语句时，它的执行顺序如下

开始协程，直接执行A语句，进入循环，直接执行B语句，执行到C语句时，挂起，知道下一帧。注意，下一帧回来，要执行的还是for循环里面的语句，因为此时还没有满足退出for循环的条件，所以这个协程的下一步就再执行一次B语句，再挂起，直到下一帧，如此反复，知道跳出for循环，此时来到了D语句，这个语句是挂起特定的时间，到达特定的时间之后，执行E语句，最终，这个协程结束。

1.6要注意的是：协程的开始不能在UPdate和lateUpdate中调用

1.7结束协程StopCoroutine（“String”）最好不用函数，会出现无法停止协程的bug，而是直接传入函数的名字字符串，协程会在其内部逻辑执行完之后自动停止。协程是一种特殊的函数，它可以在执行过程中暂停和恢复。当协程的内部逻辑执行完毕或遇到`yield break`语句时，协程会自动停止。











2、unityC#中垃圾回收的原理



3、字典



4、查找场景中的组件像FindGameObjectWithTag（"player"）这种



5、unity中两个物体进行碰撞的详细过程



6、unity中常用的数据结构如：list



7、对象池技术（不同种对象）



8、音频组件



9、委托和事件



10、C#泛型



11、Unity中的嵌套式画布



12、Json数据

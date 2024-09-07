<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.10" tiledversion="1.11.0" name="tileset" tilewidth="24" tileheight="24" tilecount="48" columns="10">
 <grid orientation="orthogonal" width="32" height="32"/>
 <transformations hflip="1" vflip="1" rotate="0" preferuntransformed="0"/>
 <image source="tileset.png" width="256" height="96"/>
 <wangsets>
  <wangset name="test-terrain" type="mixed" tile="-1">
   <wangcolor name="Water" color="#ff0000" tile="0" probability="1"/>
   <wangcolor name="Grass" color="#00ff00" tile="-1" probability="1"/>
   <wangcolor name="Stone" color="#0000ff" tile="29" probability="1"/>
   <wangtile tileid="0" wangid="1,1,0,0,0,1,1,1"/>
   <wangtile tileid="1" wangid="1,1,1,1,0,0,0,1"/>
   <wangtile tileid="10" wangid="0,0,0,1,1,1,1,1"/>
   <wangtile tileid="11" wangid="0,1,1,1,1,1,0,0"/>
  </wangset>
 </wangsets>
</tileset>

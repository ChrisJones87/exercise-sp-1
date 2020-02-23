class TilePuzzle extends React.Component {

   constructor(props) {
      super(props);

      var tileCount = 6 * 4;
      this.state = {
         tiles: [...Array(tileCount).keys()],
         selectedTile: null
      }

      this.handleTileClicked = this.handleTileClicked.bind(this);
      this.swapTile = this.swapTile.bind(this);
   }

   render() {
      const colClasses = `col-2 p-0 m-0`;

      const tiles = this.state.tiles.map((tileId, index, arr) => {
         let selectedTile = tileId === this.state.selectedTile ? true : false;

         return (
            <div key={index} className={colClasses} onClick={() => this.handleTileClicked(tileId)}>
               <Tile tileId={tileId} selectedTile={selectedTile}></Tile>
            </div>);
      });

      return (
         <div className="container-fluid">
            <h1>Tile Puzzle</h1>
            <div className="row tile-puzzle">
               {tiles}
            </div>
         </div>
      );
   }

   handleTileClicked(tileId) {
      if (this.state.selectedTile != null) {
         this.swapTile(this.state.selectedTile, tileId);
      } else {
         const newState = Object.assign({}, { ...this.state }, { selectedTile: tileId });
         this.setState(newState);
      }
   }

   swapTile(tileId1, tileId2) {
      const newTiles = [...this.state.tiles];
      const tile1Index = this.state.tiles.findIndex(tileId => tileId === tileId1);
      const tile2Index = this.state.tiles.findIndex(tileId => tileId === tileId2);

      const tile1 = newTiles[tile1Index];
      const tile2 = newTiles[tile2Index];

      newTiles[tile2Index] = tile1;
      newTiles[tile1Index] = tile2;

      const newState = Object.assign({}, { ...this.state }, { tiles: newTiles }, { selectedTile: null });
      this.setState(newState);
   }

}

class Tile extends React.Component {

   constructor(props) {
      super(props);

      const tileWidth = 600 / 6;
      const tileHeight = 400 / 4;
      const tileId = this.props.tileId;
      const top = -(Math.floor(tileId / 4)) * (tileHeight);
      const left = tileId < 6 ? -tileId * (tileWidth) : -(tileId % 6) * tileWidth;

      this.state = {
         tileWidth: 600 / 6,
         tileHeight: 400 / 4,
         top : top,
         left : left
      };
   }

   render() {
      const selectedClass = this.props.selectedTile ? "tile-selected" : "";
      const tileClasses = `tile w-100 text-center`;
      const overlayClasses= `w-100 h-100 ${selectedClass}`
      const tileId = this.props.tileId;

      const tileWidth = 600 / 6;
      const tileHeight = 400 / 4;
      const top = -(Math.floor(tileId / 6)) * (tileHeight);
      const left = tileId < 6 ? -tileId * (tileWidth) : -(tileId % 6) * tileWidth;

      const style = { backgroundPosition: `${left}px ${top}px` };
      return (
         <div className={tileClasses} style={style}>
            <div className={overlayClasses}>

            </div>
         </div>
      );
   }
}


ReactDOM.render(<TilePuzzle imageWidth={600} imageHeight={400} />, document.getElementById('content'));
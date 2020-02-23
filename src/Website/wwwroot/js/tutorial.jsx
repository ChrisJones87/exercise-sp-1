class TilePuzzle extends React.Component {

   constructor(props) {
      super(props);

      this.state = {
         tiles: [...Array(30).keys()],
         selectedTile: null
      }

      this.handleTileClicked = this.handleTileClicked.bind(this);
      this.swapTile = this.swapTile.bind(this);
   }

   render() {
      const colClasses = `col-2 p-1`;

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
   render() {
      const selectedClass = this.props.selectedTile ? "tile-selected" : "";
      const tileClasses = `tile w-100 text-center ${selectedClass}`;
      return (
         <div className={tileClasses}>
            <h1>{this.props.tileId}</h1>
         </div>
      );
   }
}


ReactDOM.render(<TilePuzzle />, document.getElementById('content'));
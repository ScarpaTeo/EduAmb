import React from 'react';
import Evento from './Evento';



class Eventos extends React.Component {


    token='IJBMYM3D3SQFZGQRLTHI';

  state = {
     eventos: [] ,
     //imagenes: []
  };

  componentWillMount(){
    this.obtenerEventos();
  }

  obtenerEventos = () =>{
      let url = `https://www.eventbriteapi.com/v3/users/me/events/?token=${this.token}`;
      fetch(url)
        .then(respuesta => respuesta.json())
        .then(info => this.setState({
          eventos: info.events,
          //imagenes: info.events.logo
        }), //console.log(this.state.imagenes)
        
        )
       
        
  }

  render() {
       return (
     <div className="container fondo">
       <div className="row justify-content-center mx-auto">
            <div className="col-md-10 info-color #33b5e5 p-1 mb-3"  style={{borderRadius: "25px",marginTop:"30px",marginLeft:"70px",marginRight:"70px"}}>
              <h5 className="h3-responsive white-text text-center mt-1">Eventos Educacion Ambiental Chaco</h5>
            </div>
       
       {this.state.eventos.map(item =>(
         <div className="col-md-12 col-lg-4">
            <Evento
              key={item}
              eventos={item}
             
              />
          </div>
       ))}
       
       </div>
     </div>
     
    );
  }
}

export default Eventos;
import React, { Component } from 'react';

export class FlatOfferCard extends Component {
    render() {
        var flatOffer = this.props.flatOffer;
        return (
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{flatOffer.Address}</h5>
                    <ul className="list-group">
                        <li className="list-group-item">Size : {flatOffer.FlatSize}</li>
                        <li className="list-group-item">Rooms : {flatOffer.NumberOfRooms}</li>
                        <li className="list-group-item">Type : {flatOffer.FlatType}</li>
                    </ul>
                    <a>{flatOffer.Url}</a>
                    <h4 className="card-text float-right">{flatOffer.Price}</h4>
                </div>
            </div>
        )
    }
}
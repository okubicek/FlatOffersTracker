import React, { Component, Fragment } from 'react';
import { FlatOfferCard } from './FlatOfferCard';
import { SearchPane } from './SearchPane';

export class FlatOffersOverview extends Component {
    constructor(props) {
        super(props);

        this.state = { flatOffers : [], loading : true };

        fetch('api/FlatOffers/Get')
            .then(response => response.json())
            .then(data => {
                this.setState({ flatOffers : data, loading : false })
            });
    }

    renderFlatOffers(flatOffers) {
        return (
            <React.Fragment>
            <div className="row">
                <SearchPane items={flatOffers} />
            </div>
            <div className="row">
                {flatOffers.map(offer =>
                    <div className="col col-sm-4">
                        <FlatOfferCard flatOffer={offer} />
                    </div>
                )}
            </div>
            </React.Fragment>
        );
    }

    render() {
        return this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderFlatOffers(this.state.flatOffers);
    }
}
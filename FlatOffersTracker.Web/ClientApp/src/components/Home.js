import React, { Component, Fragment} from 'react';
import { FlatOffersOverview } from './FlatOffersOverview';
import { SearchPane } from './SearchPane';

export class Home extends Component {
    constructor(props) {
        super(props);

        this.fetchFlatOffers = this.fetchFlatOffers.bind(this);

        this.state = { flatOffers: [], loading: true };
        this.fetchFlatOffers(null);
    }

    fetchFlatOffers(searchParams) {
        var url = new URL("api/FlatOffers/Get", "https://" + window.location.host);

        if (searchParams != null) {
            Object.keys(searchParams).forEach(key => url.searchParams.append(key, searchParams[key]));
        }

        fetch(url)
            .then(response => response.json())
            .then(data => {
                this.setState({ flatOffers: data, loading: false })
            });
    }

    static displayName = Home.name;

    render () {
        return (
            <React.Fragment>
                <div className="container fluid">
                    <SearchPane items={this.state.flatOffers}
                        handleSearch={this.fetchFlatOffers} />
                    <FlatOffersOverview
                        flatOffers={this.state.flatOffers}
                        loading={this.state.loading} />
                </div>
            </React.Fragment>
        );
    }
}
